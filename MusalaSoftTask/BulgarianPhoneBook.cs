using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MusalaSoftTask
{
    public class BulgarianPhoneBook : 
        PhoneBook<string, string>, 
        IContacts<string, string>,
        IOutgoingCalls<string>
    {
        private static readonly string NormalCountryCode = "+359";
        private static readonly string[] EquivalentCountryCode = { "0", "00359" };
        private static readonly char OperatorFristDigit = '8';
        private static readonly char[] OperatorsSecondDigit = { '7', '8', '9' };
        private static readonly char ControlDigitStart = '2';
        private static readonly char ControlDigitEnd = '9';
        private static readonly int LastDigitsCount = 6;
        private static readonly int ControlDigitCount = 1;
        private static readonly int OperatorDigitCount = 2;

        private string ChachedList = "";
        private bool ChachedHasToChange = false;

        private static readonly int TopOutgoingCallsListCount = 5;

        public BulgarianPhoneBook(string filePathAndName) : base()
        {
            StreamReader file = GetFileFromFilePathAndName(filePathAndName);
            string line;

            while ((line = file.ReadLine()) != null)
            { 
                KeyValuePair<string, string> pair;

                try
                {
                    pair = GetDataFromRecord(line);
                    string normalizedName = NormalizeName(pair.Key);
                    string value = GetValueByKey(normalizedName);
                    if (value == null)
                    {
                        ContactsList.Add(normalizedName, pair.Value);
                        OutgoingCallHistory.Add(new Tuple<int, string>(0, normalizedName));
                    }
                }
                catch (InvalidDataException)
                {
                    continue;
                }
            }

            file.Close();
            ChachedHasToChange = true;
        }

        public void AddNewPair(string name, string phoneNumber)
        {
            string normalizedName = NormalizeName(name);

            string value = GetValueByKey(normalizedName);
            if (value != null)
                throw new Exception("Key already exist");

            int counter = phoneNumber.Length - 1;
            StringBuilder data = GetPhoneNumberFromRecord(ref counter, phoneNumber);

            ContactsList.Add(normalizedName, data.ToString());
            OutgoingCallHistory.Add(new Tuple<int, string>(0, normalizedName));

            ChachedHasToChange = true;
        }

        public void DeletePairByName(string name)
        {
            string normalizedName = NormalizeName(name);

            string value = GetValueByKey(normalizedName);
            if (value == null)
                throw new KeyNotFoundException();

            ContactsList.Remove(normalizedName);
            OutgoingCallHistory.Remove(FindTuppleByKey(normalizedName));

            ChachedHasToChange = true;
        }

        public string GetPhoneNumberByName(string name)
        {
            string normalizedName = NormalizeName(name);

            string value = GetValueByKey(normalizedName);
            if (value == null)
                throw new KeyNotFoundException();

            return value;
        }

        public string ListWholePhoneBook()
        {
            if (!ChachedHasToChange)
                return ChachedList;

            StringBuilder list = new StringBuilder();
            var enumerator = ContactsList.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                list.Append(current.Key)
                    .Append(' ')
                    .Append(current.Value)
                    .Append('\n');
            }

            ChachedList = list.ToString();
            ChachedHasToChange = false;

            return ChachedList;
        }

        public void AddOutgoingCall(string name)
        {
            string normalizedName = NormalizeName(name);

            Tuple<int, string> current = FindTuppleByKey(normalizedName);
            if (current == null)
                throw new Exception("The key does not exist.");

            OutgoingCallHistory.Remove(current);
            Tuple<int, string> newTupple = new Tuple<int, string>(current.Item1 + 1, current.Item2);
            OutgoingCallHistory.Add(newTupple);
        }

        public string ListTopCalledCalls()
        {
            int counter = TopOutgoingCallsListCount;
            StringBuilder builder = new StringBuilder("");

            var enumerator = OutgoingCallHistory.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Tuple<int, string> current = enumerator.Current;
                string phoneNumber = GetValueByKey(current.Item2);

                builder.Append(current.Item2)
                        .Append(' ')
                        .Append(phoneNumber)
                        .Append(" - ")
                        .Append(current.Item1)
                        .Append(" times")
                        .Append('\n');

                if (--counter <= 0)
                    break;

            }

            return builder.ToString();
        }

        private string GetValueByKey(string key)
        {
            ContactsList.TryGetValue(key, out string value);
            return value;
        }

        private StreamReader GetFileFromFilePathAndName(string filePathAndName)
        {
            if (!File.Exists(filePathAndName))
                throw new FileNotFoundException();

            return new StreamReader(filePathAndName);
        }

        private KeyValuePair<string, string> GetDataFromRecord(string record)
        {
            int counter = record.Length - 1;

            StringBuilder phoneNumber = GetPhoneNumberFromRecord(ref counter, record);
            StringBuilder ownerName = GetPhoneOwnerFromRecord(ref counter, record);

            return new KeyValuePair<string, string> (ownerName.ToString(), phoneNumber.ToString());
        }

        private StringBuilder GetPhoneNumberFromRecord(ref int counter, string record)
        {
            StringBuilder phoneNumber = new StringBuilder("");

            phoneNumber.Insert(0, CheckLastDigits(ref counter, record));
            phoneNumber.Insert(0, CheckControlCode(ref counter, record));
            phoneNumber.Insert(0, CheckOperatorCode(ref counter, record));
            phoneNumber.Insert(0, CheckCountryCode(ref counter, record));

            return phoneNumber;
        }

        private StringBuilder GetPhoneOwnerFromRecord(ref int counter, string record)
        {
            StringBuilder builder =
                new StringBuilder(record.Substring(0, counter + 1).Trim());

            string check = builder.ToString();
            if (string.IsNullOrEmpty(check) || string.IsNullOrWhiteSpace(check))
                throw new InvalidDataException();

            return builder;
        }

        private bool CheckForValidDigitOrSpace(char digit)
        {
            if (char.IsDigit(digit) || digit.Equals('+'))
                return true;
            else if (!char.IsWhiteSpace(digit))
                throw new InvalidDataException();

            return false;
        }

        private bool CheckForValidDigit(char digit)
        {
            if (char.IsDigit(digit) || digit.Equals('+') || digit.Equals('\0'))
                return true;

            return false;
        }

        private string CheckLastDigits(ref int counter, string record)
        {
            char current;
            bool checkCurrent;
            int lastDigitsCounter = LastDigitsCount;
            StringBuilder digits = new StringBuilder("");

            while (counter > 0)
            {
                if (lastDigitsCounter <= 0)
                    return digits.ToString();

                current = record[counter--];
                checkCurrent = CheckForValidDigitOrSpace(current);

                if (lastDigitsCounter > 0 && checkCurrent)
                {
                    digits.Insert(0, current);
                    lastDigitsCounter--;
                }
            }

            throw new InvalidDataException();
        }

        private char CheckControlCode(ref int counter, string record)
        {
            char current;
            bool checkCurrent;
            int controlCounter = ControlDigitCount;

            while (counter > 0)
            {
                current = record[counter--];
                checkCurrent = CheckForValidDigitOrSpace(current);

                if (checkCurrent)
                {
                    if (ControlDigitStart <= current && current <= ControlDigitEnd)
                        return current;
                }
            }

            throw new InvalidDataException();
        }

        private String CheckOperatorCode(ref int counter, string record)
        {
            char current;
            bool checkCurrent;
            int operatorCounter = OperatorDigitCount;
            StringBuilder code = new StringBuilder("");

            while (counter > 0)
            {
                current = record[counter--];
                checkCurrent = CheckForValidDigitOrSpace(current);

                if (checkCurrent)
                {
                    if (operatorCounter == OperatorDigitCount)
                    {
                        if (!OperatorsSecondDigit.Contains(current))
                            throw new InvalidDataException();

                        code.Insert(0, current);
                        operatorCounter--;
                    }
                    else if (operatorCounter > 0)
                    {
                        if (!current.Equals(OperatorFristDigit))
                            throw new InvalidDataException();

                        return code.Insert(0, current).ToString();
                    }
                }
            }

            throw new InvalidDataException();
        }

        private string CheckCountryCode(ref int counter, string record)
        {
            bool checkCurrent = false;
            char current;

            StringBuilder rest = new StringBuilder();

            while (counter >= 0)
            {
                current = record[counter];
                try
                {
                    checkCurrent = CheckForValidDigitOrSpace(current);
                } catch (InvalidDataException)
                {
                    break;
                }

                if (checkCurrent)
                    rest.Insert(0, current);

                counter--;
            }

            string result = rest.ToString();
            bool validation = result.Equals(NormalCountryCode);
            foreach (var countryCode in EquivalentCountryCode)
                validation = validation || result.Equals(countryCode);

            if (validation)
                return NormalCountryCode;

            throw new InvalidDataException();
        }

        private string NormalizeName(string name)
        {
            return Regex
                .Replace(name, @"\s+", " ")
                .Trim()
                .ToUpper();
        }

        private Tuple<int, string> FindTuppleByKey(string key)
        {
            return OutgoingCallHistory
                .Where(
                    x => x.Item2.Equals(key)
                ).SingleOrDefault();
        }
    }
}
