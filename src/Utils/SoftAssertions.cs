using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Framework.Utils
{
    public class SoftAssertion
    {
        private readonly List<Assertion> _assertionsList = new List<Assertion>();

        public void Add<T>(T expected, T actual, string message = "") =>
            _assertionsList.Add(new Assertion(expected.ToString(), actual.ToString(), message));

        public void AssertIsTrue() =>
            AssertAll(true);

        public void AssertIsFalse() =>
            AssertAll(false);
        
        private void AssertAll(bool shouldBe)
        {
            var succeeds = true;
            var messageList = new List<string>();
            
            foreach (var assertion in _assertionsList)
            {
                if (assertion.Assert() == false)
                    succeeds = false;
                messageList.Add(assertion.GetMessage());
            }

            succeeds.Should().Be(shouldBe, String.Join("\n", messageList));
        }
    }

    class Assertion
    {
        private readonly string _expected;
        private readonly string _actual;
        private readonly string _errorMessage;
        
        public Assertion(string expected, string actual, string errorMessage)
        {
            _expected = expected;
            _actual = actual;
            _errorMessage = errorMessage;
        }

        public bool Assert() =>
            _expected.Equals(_actual);
        
        public string GetMessage()
        {
            var message = $"Expected: [{_expected}], actual: [{_actual}] - ";
            if (Assert())
                message += "success";
            else
                message += _errorMessage;
            return message;
        }
    }
}