using System;
using UnityEngine;

public class GameService
{
    private ISession session;

    private string _board = "";

    public Action<string> OnWhoIs;

    public Action<string, int> OnReceivePosition;

    public event Action<string> OnUpdateBoard;

    public event Action<string> OnPlayerTurn;

    public event Action<string> OnWinner;

    public Action ConnectToServer;


    private PopUpController _popUpController;

    public GameService(ISession ss, PopUpController popUpController)
    {
        session = ss;
        _board = "---------";
        _popUpController = popUpController;
    }

    public string Board { get => _board; private set => _board = value; }

    public GameService()
    {
        _board = "---------";
    }

    public void SetWho(string who)
    {
        session.Who = who;
        OnWhoIs?.Invoke(who);
    }

    public void ChangePosition(string player, int index) => OnReceivePosition?.Invoke(player, index);

    public void UpdateBoard(string board)
    {
        OnUpdateBoard?.Invoke(board);
        _board = board;
    }

    public void PlayerTurn(string turn) => OnPlayerTurn?.Invoke(turn);

    public void PlayerWinner(string turn) => OnWinner?.Invoke(turn);

    public void Connect() => ConnectToServer?.Invoke();


}