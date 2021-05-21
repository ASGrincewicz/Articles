
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Veganimus
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    
    public class Typing : MonoBehaviour
    {
        [SerializeField] private bool _isAutoPageTurnOn;
        [Header("UI Objects")]
        [SerializeField] private InputField _setDelayInput;
        [SerializeField] private InputField _setPageDelayInput;
        [SerializeField] private Toggle _autoTurnToggle;
        [SerializeField] private TMP_Text _screenText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _previousPageButton;
        [Header("Setting")]
        [Multiline(5)]
        [SerializeField] private List<string> _pages = new List<string>();
        [Multiline(5)]
        [SerializeField] private string _typedText;
        [SerializeField] private float _delayTime;
        [SerializeField] private float _pageDelayTime;
        private bool _isTyping;
        private int _index = 0;
        private int _currentPage =0;
        private int _textLength;
        private string _textToType;
        private char _characterToType;
        private WaitForSeconds _characterDelay;
        private WaitForSeconds _pageTurnDelay;
       
        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartTyping);
            _nextPageButton.onClick.AddListener(UserTurnedNextPage);
            _previousPageButton.onClick.AddListener(UserTurnedPreviousPage);
            _autoTurnToggle.onValueChanged.AddListener(AutoTurn);
            _setDelayInput.onEndEdit.AddListener(SetDelay);
            _setPageDelayInput.onEndEdit.AddListener(SetPageDelay);
            _textLength = _pages[_currentPage].Length;
            _characterDelay = new WaitForSeconds(_delayTime);
            _pageTurnDelay = new WaitForSeconds(_pageDelayTime);
        }
        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartTyping);
            _nextPageButton.onClick.RemoveListener(UserTurnedNextPage);
            _previousPageButton.onClick.RemoveListener(UserTurnedPreviousPage);
            _autoTurnToggle.onValueChanged.RemoveListener(AutoTurn);
            _setDelayInput.onEndEdit.RemoveListener(SetDelay);
            _setPageDelayInput.onEndEdit.RemoveListener(SetPageDelay);
        }
        private void AutoTurn(bool isOn) => _isAutoPageTurnOn = isOn;
         //Allows user to set the delay between characters.
        private void SetDelay(string newDelay)
        {
            _delayTime = Convert.ToSingle(newDelay);
            _characterDelay = new WaitForSeconds(_delayTime);
        }
        // Allows user to set the delay between page turns.
        private void SetPageDelay(string newPageDelay)
        {
            _pageDelayTime = Convert.ToSingle(newPageDelay);
            _pageTurnDelay = new WaitForSeconds(_pageDelayTime);
        }
        // This is called when the 'Next Page' button is pressed, so it doesn't interfere with Auto-turn.
        private void UserTurnedNextPage()
        {
            _isAutoPageTurnOn = false;
            _autoTurnToggle.isOn = _isAutoPageTurnOn;
            NextPage();
        }
        // This is called when the 'Previous Page' button is pressed, so it doesn't interfere with Auto-turn.
        private void UserTurnedPreviousPage()
        {
            _isAutoPageTurnOn = false;
            _autoTurnToggle.isOn = _isAutoPageTurnOn;
            PreviousPage();
        }
        // Called when the 'Start Typing' button is pressed. Could be called another way.
        private void StartTyping() => StartCoroutine(TypeCharacter());
        //Called when current page is done typing if Auto-turn is on, otherwise called by user input.
        private void NextPage()
        {
            if (_currentPage < _pages.Count - 1)
            {
                if (!_isTyping)
                {
                    _screenText.text = "";
                    _typedText = "";
                    _textToType = _pages[_currentPage++];
                    _textLength = _pages[_currentPage].Length;
                    StartCoroutine(TypeCharacter());
                }
            }
        }
          //Called when current page is done typing if Auto-turn is on, otherwise called by user input.
        private void PreviousPage()
        {
            if (_currentPage != 0)
            {
                if (!_isTyping)
                {
                    _screenText.text = "";
                    _typedText = "";
                    _textToType = _pages[_currentPage--];
                    _textLength = _pages[_currentPage].Length;
                    StartCoroutine(TypeCharacter());
                }
            }
        }
        //This iterates through the characters in the assigned string and displays the characters one by one with a user-assigned delay between.
        private IEnumerator TypeCharacter()
        {
            _isTyping = true;
            if (_textLength > _typedText.Length)
            {
                for (int index = 0; index < _textLength; index++)
                {
                    _characterToType = _pages[_currentPage][index];
                    _typedText = $"{_typedText += _characterToType}";
                    _screenText.text = $"{_typedText}";
                    yield return _characterDelay;
                }
            }
            if (_isAutoPageTurnOn)
            {
                if (_currentPage < _pages.Count - 1)
                {
                    yield return _pageTurnDelay;
                    _isTyping = false;
                    NextPage();
                }
                else
                    _isTyping = false;
            }
            else
                _isTyping = false;
        }
    }
}
