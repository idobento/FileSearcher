using System;
using System.IO;

namespace ExceptionsandValidaton
{
    public class validationTest
    {
        public event Action<string> ValidationError;
        public bool isFileValid(string Value)
        {
            bool validationResult = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                {
                    validationResult = false;
                    throw new NullReferenceException("Error \n The file name you entered is empty");
                }
                if (Value.ContainsAny("*", "/", "\"", ":", ".", ";", "<", ">", "|", "'", "?"))
                // the function containsAny is from nudget called "Z.ExtensionMethods"
                {
                    validationResult = false;
                    throw new ArgumentException("Error \nFile name cannot contains: * , \" , / , : , . , ; , < , > , | , ' or ?");
                }
            }
            catch (NullReferenceException e)
            {
                ValidationError?.Invoke(e.Message);
            }
            catch (ArgumentException e)
            {
                ValidationError?.Invoke(e.Message);
            }
            return validationResult;
        }
        public bool isDirectoryValidAndExist(string Value)
        {
            bool validationResult = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                {
                    validationResult = false;
                    throw new NullReferenceException("Error \n The file name you entered is empty");
                }
                if (!Directory.Exists(Value))
                {
                    validationResult = false;
                    throw new DirectoryNotFoundException("Error \nDirectory " + Value + " does not exist." +
                    " \nNotice that you need to enter the full path, for example:\"c:\\\\temp\"");
                }
            }
            catch (NullReferenceException e)
            {
                ValidationError?.Invoke(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                ValidationError?.Invoke(e.Message);
            }
            return validationResult;
        }
    }
}

