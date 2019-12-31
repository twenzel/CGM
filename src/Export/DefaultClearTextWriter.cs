﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Export
{
    public class DefaultClearTextWriter : IClearTextWriter, IDisposable
    {
        private StreamWriter _writer;
        private List<Message> _messages = new List<Message>();
        private Command _currentCommand;        

        private const string LINE_FEED = "\n";
        private const int MAX_CHARS_PER_LINE = 80;
        private int current_chars_per_line;
        private const char SPACE_CHARACTER = ' ';
        private const char PLUS_CHARACTER = '+';
        private const char MINUS_CHARACTER = '-';
        private const char NUMBER_SIGN = '#';
        private const char SEMICOLON_CHARACTER = ';';
        private const char SLASH_CHARACTER = '/';
        private const char OPEN_PARENTHESIS_CHARACTER = '(';
        private const char CLOSE_PARENTHESIS_CHARACTER = ')';
        private const char COMMA_CHARACTER = ',';
        private const char DECIMAL_POINT_CHARACTER = '.';
        private const char SINGLE_QUOTE_CHARACTER = '\'';
        private const char DOUBLE_QUOTE_CHARACTER = '"';
        private const char UNDERSCORE_CHARACTER = '_';
        private const char DOLLAR_SIGN_CHARACTER = '$';
        private const char PERCENT_CHARACTER = '%';
        private const char LINE_FEED_CHARACTER ='\n';

        public IEnumerable<Message> Messages => _messages;  

        public DefaultClearTextWriter(Stream stream)
        {
            _writer = new StreamWriter(stream, CodePagesEncodingProvider.Instance.GetEncoding(1252));
        }       

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void WriteLine(string line)
        {
            Write(line);
            Write(LINE_FEED);
            current_chars_per_line = 0;
        }

        public void WriteCommand(Command command)
        {
            _currentCommand = command;
            command.WriteAsClearText(this);
            _currentCommand = null;
        }


        public void Write(string text)
        {          
            if (text.Contains(LINE_FEED) && text.Length > 1)
            {
                var lines = text.Split(new[] { LINE_FEED }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length -1; i++)
                {
                    WriteLine(lines[i]);
                }

                Write(lines[lines.Length - 1]);
            }
            else
            {

                if (current_chars_per_line + text.Length > MAX_CHARS_PER_LINE)
                {
                    if (text == LINE_FEED || text == ";" || text.Length == 1)
                    {
                        _writer.Write(text);
                    }
                    else
                    {
                        while (current_chars_per_line + text.Length > MAX_CHARS_PER_LINE && text.Length > 0)
                        {
                            var nextSeparatorChar = text.LastIndexOf(" ", MAX_CHARS_PER_LINE - current_chars_per_line);
                            if (nextSeparatorChar > 0 && text.Length > nextSeparatorChar)
                            {
                                // if this is the separator between command and content (like "mfdesc 'abc'")
                                // then ignore this and put out the whole line at once
                                if (text[nextSeparatorChar + 1] == '\'')
                                    nextSeparatorChar = -1;
                            }

                            if (nextSeparatorChar == -1)
                                nextSeparatorChar = text.LastIndexOf(LINE_FEED, MAX_CHARS_PER_LINE - current_chars_per_line);

                            if (nextSeparatorChar == -1)
                                nextSeparatorChar = text.IndexOf(" ");

                            if (nextSeparatorChar == -1)
                                nextSeparatorChar = text.IndexOf(LINE_FEED);

                            if (nextSeparatorChar > 0)
                            {
                                var currentLine = text.Substring(0, nextSeparatorChar);
                                text = text.Substring(nextSeparatorChar);
                                WriteLine(currentLine);
                            }                            
                            else
                            {
                                _writer.Write(text);                                
                                current_chars_per_line = 0;
                                text = "";
                            }
                        }

                        Write(text);
                    }
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    _writer.Write(text);
                    current_chars_per_line += text.Length;
                }
            }
        }

        public void Info(string message)
        {
            if (_currentCommand != null)
                _messages.Add(new Message(Severity.Info, _currentCommand.ElementClass, _currentCommand.ElementId, message, _currentCommand.ToString()));
            else
                _messages.Add(new Message(Severity.Info, 0, 0, message, ""));
        }

        //private static string FilterCharacters(string input)
        //{
        //    var sb = new StringBuilder();
        //    bool isStringParameterStarted = false;
        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        char c = input[i];

        //        if (c == SINGLE_QUOTE_CHARACTER)
        //            isStringParameterStarted = !isStringParameterStarted;

        //        if (isStringParameterStarted || IsValidChar(c))
        //            sb.Append(c);
        //    }
        //    return sb.ToString();
        //}

        //private static bool IsValidChar(char value)
        //{
        //    return (Char.IsLetter(value) && value < 128)
        //        || char.IsDigit(value)
        //        || value == SPACE_CHARACTER
        //        || value == PLUS_CHARACTER
        //        || value == MINUS_CHARACTER
        //        || value == NUMBER_SIGN
        //        || value == SEMICOLON_CHARACTER
        //        || value == SLASH_CHARACTER
        //        || value == OPEN_PARENTHESIS_CHARACTER
        //        || value == CLOSE_PARENTHESIS_CHARACTER
        //        || value == COMMA_CHARACTER
        //        || value == DECIMAL_POINT_CHARACTER
        //        || value == SINGLE_QUOTE_CHARACTER
        //        || value == DOUBLE_QUOTE_CHARACTER
        //        || value == UNDERSCORE_CHARACTER
        //        || value == DOLLAR_SIGN_CHARACTER
        //        || value == PERCENT_CHARACTER
        //        || value == LINE_FEED_CHARACTER;
        //}

    }
}
