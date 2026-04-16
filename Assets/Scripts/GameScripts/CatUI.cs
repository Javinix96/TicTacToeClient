using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CatUI : MonoBehaviour
{
    private GameService _gameService;
    private ISession _client;
    [SerializeField]
    private GameObject gridLayout;
    [SerializeField]
    private GameObject cellImg;
    [SerializeField]
    private GameObject O;
    [SerializeField]
    private GameObject X;
    [SerializeField]
    private Transform spawnTurn;

    [SerializeField]
    private GameObject xPlayer;
    [SerializeField]
    private GameObject oPlayer;
    [SerializeField]
    private TextMeshProUGUI turnTxt;

    [SerializeField]
    private Button readyButtom;

    private BoardCell[] board;
    private int currentIndex;

    public int CurrentIndex
    {
        get => currentIndex; set => currentIndex = value;
    }

    void SpawnCells()
    {
        for (int i = 0; i < board.Length; i++)
            board[i] = SpawnCell(i);
    }

    BoardCell SpawnCell(int index)
    {
        GameObject o = Instantiate(cellImg);
        o.transform.SetParent(gridLayout.transform);
        return InitCell(o, index);
    }

    BoardCell InitCell(GameObject gm, int index)
    {
        BoardCell cell = gm.AddComponent<BoardCell>();
        cell.Init(_client, this);
        cell.Index = index;
        cell.CanClick = true;
        return cell;
    }

    void ChangeSize()
    {
        GridLayoutGroup glg = gridLayout.GetComponent<GridLayoutGroup>();
        int width = Camera.main.pixelWidth;
        int size = (width - 120) / 3;
        glg.cellSize = new Vector2(size, size);
    }

    public async Task Initilize(ISession session, GameService gm)
    {
        _client = session;
        _gameService = gm;
        board = new BoardCell[9];
        ChangeSize();
        SpawnCells();
        SetActions();
    }

    private void SetActions()
    {
        _gameService.OnWhoIs += SetTurn;
        _gameService.OnReceivePosition += PlayerPosition;
        _gameService.OnUpdateBoard += OnUpdateBoard;
        _gameService.OnPlayerTurn += PlayerTurnText;
        _gameService.OnWinner += SayWinner;
    }

    public void SetCirclePosition(Vector2 pos)
    {
        RectTransform rect = O.GetComponent<RectTransform>();
        rect.position = pos;
    }

    private void SetTurn(string turn)
    {
        GameObject go = null;
        RectTransform rect = null;
        if (turn.Equals("X"))
        {
            go = Instantiate(X, spawnTurn.position, Quaternion.identity);
            go.transform.SetParent(spawnTurn);
            rect = go.GetComponent<RectTransform>();
            rect.localScale = new Vector3(4, 4);
            return;
        }

        go = Instantiate(O, spawnTurn.position, Quaternion.identity);
        go.transform.SetParent(spawnTurn);
        rect = go.GetComponent<RectTransform>();
        rect.localScale = new Vector3(2, 2);
    }

    private void PlayerPosition(string player, int index)
    {
        if (player.Equals("X"))
            MoveX(index);
        else
            MoveO(index);

        CurrentIndex = index;
    }

    private void MoveX(int index)
    {
        BoardCell cell = board[index];

        if (cell == null) return;

        if (xPlayer == null) return;

        MovePlayer(xPlayer, cell);

    }

    private void MoveO(int index)
    {
        BoardCell cell = board[index];

        if (cell == null) return;

        if (oPlayer == null) return;

        MovePlayer(oPlayer, cell);
    }

    private void MovePlayer(GameObject playerObj, BoardCell pos)
    {
        RectTransform rect;
        rect = playerObj.gameObject.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1.5f, 1.5f);
        rect.position = pos.gameObject.GetComponent<RectTransform>().position;
    }

    private void OnUpdateBoard(string board)
    {
        var clientBoard = _gameService.Board.ToCharArray();
        var serverBoard = board.ToCharArray();

        if (clientBoard.Length != serverBoard.Length)
        {
            Debug.Log("Error en cargar el tablero");
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (clientBoard[i] != '-') continue;

            if (serverBoard[i] == '-') continue;

            CreateBoardPos(serverBoard[i], i);
        }
    }

    private void CreateBoardPos(char pl, int index)
    {
        BoardCell cell = board[index];

        if (cell == null) return;

        if (pl == 'X')
        {
            RectTransform rect;
            GameObject x = Instantiate(X);
            x.transform.SetParent(this.transform);
            rect = x.gameObject.GetComponent<RectTransform>();
            rect.localScale = new Vector3(2.3f, 2.3f);
            rect.position = cell.gameObject.GetComponent<RectTransform>().position;

            xPlayer.transform.position = new Vector2(10000, 0);
        }
        else
        {
            RectTransform rect;
            GameObject o = Instantiate(O);
            o.transform.SetParent(this.transform);
            rect = o.gameObject.GetComponent<RectTransform>();
            rect.localScale = new Vector3(2.3f, 2.3f);
            rect.position = cell.gameObject.GetComponent<RectTransform>().position;
            oPlayer.transform.position = new Vector2(10000, 0);
        }
    }

    public void SendReady() => _client.send(PacketFactory.SendPacketPos(PacketTypeSend.SendReadyPos, _client.RoomID, _client.Who, currentIndex));

    public void PlayerTurnText(string turn) => turnTxt.text = turn;

    public void SayWinner(string message)
    {
        turnTxt.text = message;
        readyButtom.enabled = false;
    }
}
