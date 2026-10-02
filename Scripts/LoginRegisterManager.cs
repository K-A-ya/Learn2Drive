using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginRegisterManager : MonoBehaviour
{
    [Header("input fields")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> userCollection;

    void Start()
    {
        // connect to mongodb (CHANGE PASSWORD)
        string connectionString = 
        client = new MongoClient(connectionString);

        // connect to database (chk spelling)
        database = client.GetDatabase("loginL2D");

        // connect to collection(chk spelling)
        userCollection = database.GetCollection<BsonDocument>("users");
    }

    // password validation system 
    private bool IsPasswordStrong(string password)
    {
        if (password.Length < 8)
            return false;
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;
        if (!Regex.IsMatch(password, @"[a-z]"))
            return false;
        if (!Regex.IsMatch(password, @"[0-9]"))
            return false;
        if (!Regex.IsMatch(password, @"[\W_]"))
            return false;

        return true;
    }

    public async void OnRegisterClicked()
    {
        // validation system responce via debul log or console
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("username or password cannot be empty");
            return;
        }

        if (!IsPasswordStrong(password))
        {
            Debug.LogWarning("password must be at least 8 characters with uppercase, lowercase, number, and special symbol");
            return;
        }

        var existingUser = await userCollection.Find(Builders<BsonDocument>.Filter.Eq("username", username)).FirstOrDefaultAsync();

        if (existingUser != null)
        {
            Debug.LogWarning("user already exists");
            return;
        }

        var newUser = new BsonDocument
        {
            { "username", username },
            { "password", password }
        };

        await userCollection.InsertOneAsync(newUser);
        Debug.Log("user registered successfully");

        SceneManager.LoadScene("Main Menu");
    }

    public async void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("username", username),
            Builders<BsonDocument>.Filter.Eq("password", password)
        );

        var user = await userCollection.Find(filter).FirstOrDefaultAsync();

        if (user != null)
        {
            Debug.Log("login successful");
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            Debug.LogWarning("invalid username or password");
        }
    }
}
