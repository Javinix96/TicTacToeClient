using System.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private uint id = 0;
    private RoomInfo roomInfo;
    public uint ID { set { id = value; } get => id; }
    public RoomInfo Room { set { roomInfo = value; } get => roomInfo; }

    public void Start()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public async Task SaveName(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData/PlayerData.txt");

        PlayerData data = new PlayerData()
        {
            PlayerName = name
        };

        string json = JsonUtility.ToJson(data);

        try
        {
            using (StreamWriter writter = new StreamWriter(path, false, Encoding.UTF32))
            {
                await writter.WriteAsync(json);
                writter.Close();
            }
        }
        catch (Exception e)
        {
            print(e.Message);
        }
    }

    public async Task<PlayerData> GetPlayerData()
    {
        string folder = Path.Combine(Application.persistentDataPath, "PlayerData");
        string path = Path.Combine(folder, "PlayerData.txt");
        string json = "";

        try
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(path))
                using (File.Create(path)) { }

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                    json = reader.ReadLine();
            }
        }
        catch (Exception e)
        {
            print(e.Message);
        }

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        return data;
    }
}
