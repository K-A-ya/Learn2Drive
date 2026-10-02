
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

// time to get quizical
public class QuizController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject topicSelectionPanel; // panel with 3 buttons: Signs, Laws, Rules // *CAPITALIZATION* dont mess it up
        public GameObject quizPanel;           // main quiz UI
    public GameObject finishedPanel;       // final summary

    [Header("UI Elements (Quiz Panel)")]
    public TextMeshProUGUI questionField;  // shows current question
    public TMP_InputField answerField;          // user's answer input
    public Button submitButton;         //submit answer for verification NOT validation
    public Button nextButton;           // next question
    public TextMeshProUGUI feedbackText;   // shows correctness + explanation
    public TextMeshProUGUI progressText;   // "Question X / Y"
    public TextMeshProUGUI scoreText;      // show running score

    [Header("Finished UI")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalSummaryText;

    // Internal state
    private string currentTopic = "";
    private int difficultyIndex = 0;   // 0 --> 4 for progressive difficulty *success criteria
    private int questionIndex = 0;     // index inside current difficulty array
    private int score = 0;
    private int totalQuestions = 0;

    // Questions: topic -> difficulty -> array of questions
    // Each question has a matching answer and (optional) explanation
    private Dictionary<string, string[][]> questions;
    private Dictionary<string, string[][]> answers;
    private Dictionary<string, string[][]> explanations;

    void Awake()
    {
        InitializeDatabase();
        // UI initial state / STARTUP 
        topicSelectionPanel.SetActive(true);
        quizPanel.SetActive(false);
        finishedPanel.SetActive(false);

        submitButton.onClick.AddListener(OnSubmitPressed);
        nextButton.onClick.AddListener(OnNextPressed);
        nextButton.gameObject.SetActive(false); // hidden until after submit
    }

    void InitializeDatabase()
    {
        // Create dictionaries and populate with 5 difficulty arrays each. (for now ig)
        questions = new Dictionary<string, string[][]>();
        answers = new Dictionary<string, string[][]>();
        explanations = new Dictionary<string, string[][]>();

        // --- SIGNS ----
        questions["Signs"] = new string[][]
        {
            // up to diff 4 should all be correct for a pass 
            // difficulty 0 (easy)
            new string[]
            {
                "What does a red octagon sign mean?",
                "What does a triangular sign with a red border usually indicate?",
                "Which sign means 'No entry'?"
            },
            // difficulty 1
            new string[]
            {
                "What does a circular blue sign with a white arrow indicate?",
                "What does a yellow diamond sign typically mean in some countries?",
                "What sign warns of a pedestrian crossing?"
            },
            // difficulty 2
            new string[]
            {
                "What does a circular sign with a red border and a number mean?",
                "What does a red circle with a white horizontal bar indicate?",
                "What sign shows that you must give way?"
            },
            // difficulty 3 (med)
            new string[]
            {
                "What is the meaning of a sign showing a truck with a red line through it?",
                "Describe the meaning of a white sign with black diagonal stripes.",
                "What does a sign with a bicycle inside a red circle mean?"
            },
            // difficulty 4 (hard)
            new string[]
            {
                "What does a blue rectangular sign with white letters 'M' usually indicate in some countries?",
                "What is the purpose of an 'End of all restrictions' round sign?",
                "Explain the meaning of a flashing amber traffic light at a pedestrian crossing."
            }
        };

            // organized as q --> a or a-->q
        answers["Signs"] = new string[][]
        {
            new string[] { "stop", "warning", "no entry" },
            new string[] { "mandatory direction", "warning", "pedestrian crossing" },
            new string[] { "speed limit", "no entry", "give way" },
            new string[] { "no heavy goods vehicles / no trucks", "end of restrictions / national speed limit", "no bicycles" },
            new string[] { "motorway", "end of restrictions", "proceed with caution; give way to pedestrians" }
        };

        explanations["Signs"] = new string[][]
        {
            new string[] { "A red octagon is universally 'Stop'.", "Triangular red-border signs are warnings.", "A red circle with white dash or a red circle with white rectangle often means 'No entry'." },
            new string[] { "Blue circle with arrow = mandatory direction.", "Yellow diamond often indicates priority or warning depending on region.", "Marked sign or zebra symbols warn of crossings." },
            new string[] { "Numbers inside red circle indicate speed limits.", "Red circle with white horizontal bar = No entry.", "Give way signs are inverted triangle or 'Yield'." },
            new string[] { "Truck with red line = vehicles of that type prohibited.", "White with black stripes often signifies end of previous restrictions.", "Bicycle in red circle = bicycles prohibited." },
            new string[] { "M = motorway in many countries.", "End of all restrictions clears prior speed and passing rules.", "Flashing amber = stop when pedestrians are present or be prepared to stop; give way to pedestrians." }
        };

        // -- LAWS --
        questions["Laws"] = new string[][]
        {
            new string[] {
                "What is the typical UK national speed limit for single carriageway roads (mph)?",
                "Should you use a mobile phone while driving without hands-free? (yes/no)",
                "Who has priority at a roundabout?"
            },
            new string[] {
                "At what blood alcohol limit is it illegal to drive in many countries? (answer with 'low'/'very low'/'0.08' etc)",
                "When approaching a zebra crossing with pedestrians waiting, what should you do?",
                "Is it legal to overtake on the left in general? (yes/no)"
            },
            new string[] {
                "When must you use dipped headlights?",
                "What is the minimum following distance rule (in seconds) in good conditions?",
                "If an emergency vehicle approaches with lights on, what should you do?"
            },
            new string[] {
                "What documents must you carry when driving (three items)?",
                "What is the rule for child car seats up to 12 years or 135cm in many countries?",
                "What is 'constructive driving' offence commonly called (short phrase)?"
            },
            new string[] {
                "Explain what 'drink-drive' laws intend to prevent (short).",
                "Describe the consequences of refusing a breath test in many jurisdictions.",
                "When can a provisional license holder carry passengers (brief)?"
            }
        };

        answers["Laws"] = new string[][]
        {
            new string[] { "60", "no", "vehicles on roundabout/traffic on roundabout" },
            new string[] { "0.08", "stop and let them cross/slow and give way", "no" },
            new string[] { "in poor visibility/at night when required", "2", "pull over and let them pass / give way" },
            new string[] { "license insurance v5 (or registration) or id", "use appropriate child restraint/booster", "dangerous driving" },
            new string[] { "prevent impaired driving / reduce accidents", "penalties, fines, licence loss or arrest", "when supervised/instructor (varies by country)" }
        };

        explanations["Laws"] = new string[][]
        {
            new string[] { "UK single carriageway national limit is 60mph for cars unless signed otherwise.", "Using handheld phone is illegal and distracts the driver.", "Traffic already on the roundabout usually has priority." },
            new string[] { "Common legal limit is 0.08% BAC in some countries (varies).", "Give way to pedestrians; stop if they are crossing.", "Overtaking on left is generally not allowed except in special cases." },
            new string[] { "Dipped headlights at night or poor visibility.", "A 2 second rule is a common minimum in good conditions; increase when wet.", "Move aside safely and allow emergency vehicle through." },
            new string[] { "Carry driving license, insurance, and vehicle registration/document as a minimum (depends on country).", "Children should use appropriate child restraint/booster until 12 years or 135cm in many regions.", "Dangerous driving is the common legal term for very poor driving." },
            new string[] { "They reduce the risk of accidents due to intoxication and protect others on the road.", "Refusal often has severe consequences: licence suspension, fines, or arrest (varies).", "Often only with an approved supervisor or not allowed — depends on the country's rules." }
        };

        // --- RULES ---
        questions["Rules"] = new string[][]
        {
            new string[] {
                "Which side of the road do you drive on in the UK?",
                "When do you indicate? (short)",
                "What does double yellow line at the kerb usually mean?"
            },
            new string[] {
                "At an uncontrolled junction, who should you give way to?",
                "What is the purpose of a box junction?",
                "When can you use the hard shoulder in emergencies?"
            },
            new string[] {
                "What does lane discipline mean (brief)?",
                "When merging, who should give way?",
                "What should you check before opening a car door on the traffic side?"
            },
            new string[] {
                "Explain purpose of chevrons on a motorway slip road.",
                "When parking uphill with no curb, which way to steer?",
                "What is mirror-signal-manoeuvre sequence?"
            },
            new string[] {
                "Describe what 'undertaking' means and if it is allowed.",
                "Explain 'staggered junction' briefly.",
                "What is the correct procedure when an obstruction blocks the lane?"
            }
        };

        answers["Rules"] = new string[][]
        {
            new string[] { "left", "to signal intention / when turning or changing lane", "no parking" },
            new string[] { "traffic from right (or main road)", "prevent blocking junctions", "only in breakdowns/emergency" },
            new string[] { "keeping to your lane and only changing when safe", "the merging traffic should give way unless signed", "look and check mirrors and blind spot" },
            new string[] { "guide/speed control on slip road / keep safe approach", "turn wheels away from road (or into verge) - depends on direction; for UK steer away from kerb downhill = towards left? (common guidance: uphill turn towards curb if there is one)", "check mirrors, signal intent, then carry out manoeuvre" },
            new string[] { "undertaking = passing on the inside; usually not allowed except in slow queue", "two staggered T junctions arranged to avoid direct cross traffic", "slow, signal, and rejoin when safe; follow diversions" }
        };

        explanations["Rules"] = new string[][]
        {
            new string[] { "UK drives on the left. Indicate to show turning/changing lanes. Double yellow lines usually mean no parking at any time (subject to local signs).", "At uncontrolled junctions you usually give way to traffic from the right in most countries (UK: priority rules). Box junctions keep junctions clear. Hard shoulder is for emergencies only.", "Lane discipline = keep lane unless overtaking/turning; merging rules prevent collisions; check blind spots to avoid harming cyclists." }
        };

        // organize in future properly
        //questions from gov.uk
    }

    // Button hook: called by the 3 topic buttons with parameter "Signs", "Laws", or "Rules" I LOVE THIS
    public void SelectTopic(string topicName)
    {
        if (!questions.ContainsKey(topicName))
        {
            Debug.LogError("Topic not found: " + topicName); //caps are important
            return;
        }

        currentTopic = topicName;
        topicSelectionPanel.SetActive(false);
        quizPanel.SetActive(true);
        finishedPanel.SetActive(false);

        // reset state
        difficultyIndex = 0;
        questionIndex = 0;
        score = 0;
        totalQuestions = CountTotalQuestionsForTopic(topicName);

        UpdateScoreUI();
        ShowCurrentQuestion();
    }

    int CountTotalQuestionsForTopic(string topic)
    {
        int total = 0;
        var arr = questions[topic];
        for (int i = 0; i < arr.Length; i++)
            total += arr[i].Length;
        return total;
    }

    void ShowCurrentQuestion()
    {
        // Defensive checks
        if (difficultyIndex >= 5)
        {
            FinishQuiz();
            return;
        }

        string[] difficultyArray = questions[currentTopic][difficultyIndex];
        if (questionIndex >= difficultyArray.Length)
        {
            // move to next difficulty
            difficultyIndex++;
            questionIndex = 0;
            ShowCurrentQuestion();
            return;
        }

        string q = difficultyArray[questionIndex];
        questionField.text = $"(Topic: {currentTopic}) [Difficulty {difficultyIndex + 1}/5]\n\n{q}";
        answerField.text = "";
        feedbackText.text = "";
        submitButton.interactable = true;
        nextButton.gameObject.SetActive(false);
        UpdateProgressUI();
    }

    void UpdateProgressUI()
    {
        int askedSoFar = 0;
        for (int d = 0; d < difficultyIndex; d++)
            askedSoFar += questions[currentTopic][d].Length;
        askedSoFar += questionIndex + 1;
        progressText.text = $"Question {askedSoFar} / {totalQuestions}";
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {score}";
    }

    public void OnSubmitPressed()
    {
        submitButton.interactable = false;
        string userAnswer = answerField.text.Trim().ToLower();
        string correct = "";

        // fetch correct answer (if available)
        if (answers.ContainsKey(currentTopic) && difficultyIndex < answers[currentTopic].Length)
        {
            var ansArray = answers[currentTopic][difficultyIndex];
            if (questionIndex < ansArray.Length)
                correct = ansArray[questionIndex].ToLower();
        }

        bool isCorrect = false;
        if (!string.IsNullOrEmpty(correct))
        {
            // simple matching: contains or equals (to allow slight variations)
            if (userAnswer == correct || userAnswer.Contains(correct) || correct.Contains(userAnswer))
                isCorrect = true;
        }
        else
        {
            // If no answer stored, mark as false but show message
            feedbackText.text = "No model answer available for this question.";
        }

        if (isCorrect)
        {
            score++;
            feedbackText.text = "<b>Correct!</b>\n\n";
        }
        else
        {
            feedbackText.text = "<b>Incorrect.</b>\n\n";
            // show expected short answer if present (helpful)
            if (!string.IsNullOrEmpty(correct))
                feedbackText.text += $"Expected: {correct}\n\n";
        }

        // add explanation if available
        if (explanations.ContainsKey(currentTopic)
            && difficultyIndex < explanations[currentTopic].Length
            && questionIndex < explanations[currentTopic][difficultyIndex].Length)
        {
            feedbackText.text += "Explanation: " + explanations[currentTopic][difficultyIndex][questionIndex];
        }

        nextButton.gameObject.SetActive(true);
        UpdateScoreUI();
    }

    public void OnNextPressed()
    {
        // move to next question within difficulty or next difficulty
        questionIndex++;
        // if passed end-of-difficulty, ShowCurrentQuestion handles increment
        ShowCurrentQuestion();
    }

    void FinishQuiz()
    {
        quizPanel.SetActive(false);
        finishedPanel.SetActive(true);
        finalScoreText.text = $"Final score: {score} / {totalQuestions}";
        finalSummaryText.text = $"You completed the {currentTopic} test.\nCorrect: {score}\nIncorrect: {totalQuestions - score}";
    }

    // optional: hook this to a "Retry" button in finishedPanel
    public void RetryTopic()
    {
        topicSelectionPanel.SetActive(false);
        finishedPanel.SetActive(false);
        quizPanel.SetActive(true);
        difficultyIndex = 0;
        questionIndex = 0;
        score = 0;
        totalQuestions = CountTotalQuestionsForTopic(currentTopic);
        UpdateScoreUI();
        ShowCurrentQuestion();
    }

    // optional: back to topic selection
    public void BackToTopics()
    {
        topicSelectionPanel.SetActive(true);
        quizPanel.SetActive(false);
        finishedPanel.SetActive(false);
    }
}
