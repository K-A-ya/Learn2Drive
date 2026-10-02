using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    // array of questions per topic? or difficulty based?
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public TMP_Dropdown topicDropdown;

    private string selectedTopic;
    private int difficultyLevel = 0;

    private Dictionary<string, List<string[]>> topicQuestions = new Dictionary<string, List<string[]>>();
    private Dictionary<string, List<string[]>> topicAnswers = new Dictionary<string, List<string[]>>();

    private int currentQuestionIndex = 0;

    void Start()
    {
        SetupQuestions();
        topicDropdown.onValueChanged.AddListener(delegate { OnTopicSelected(); });
    }

    void SetupQuestions()
    // per topic 6 total questions
    {
        topicQuestions["Driving Laws"] = new List<string[]> {
            new string[] { "What is the legal alcohol limit in mg?", "What age can you get a learner's license?" },
            new string[] { "What does a double yellow line mean?", "Max speed limit in urban areas?" },
            new string[] { "When can you overtake on a single line?", "Penalties for reckless driving?" }
        };

        topicAnswers["Driving Laws"] = new List<string[]> {
            new string[] { "80", "16" },
            new string[] { "no overtaking", "50" },
            new string[] { "when clear", "license suspension" }
        };

        topicQuestions["Road Signs"] = new List<string[]> {
            new string[] { "What does a red octagon mean?", "What does a triangle mean?" },
            new string[] { "What does a circular blue sign mean?", "What does a yellow diamond mean?" },
            new string[] { "Explain the difference between regulatory and warning signs?", "When do you ignore signs?" }
        };

        topicAnswers["Road Signs"] = new List<string[]> {
            new string[] { "stop", "yield" },
            new string[] { "mandatory", "caution" },
            new string[] { "rules warning", "never" }
        };
    }

    public void OnTopicSelected()
    {
        selectedTopic = topicDropdown.options[topicDropdown.value].text;
        difficultyLevel = 0;
        currentQuestionIndex = 0;
        ShowQuestion();
    }

    public void CheckAnswer()
    {
        string userAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = topicAnswers[selectedTopic][difficultyLevel][currentQuestionIndex].ToLower();

        if (userAnswer == correctAnswer)
        {
            difficultyLevel = Mathf.Min(difficultyLevel + 1, topicQuestions[selectedTopic].Count - 1);
        }
        else
        {
            difficultyLevel = Mathf.Max(difficultyLevel - 1, 0);
        }

        currentQuestionIndex = (currentQuestionIndex + 1) % topicQuestions[selectedTopic][difficultyLevel].Length;
        ShowQuestion();
    }

    void ShowQuestion()
    {
        string question = topicQuestions[selectedTopic][difficultyLevel][currentQuestionIndex];
        questionText.text = question;
        answerInput.text = "";
    }
}
