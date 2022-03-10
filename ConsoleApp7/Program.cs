// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.Collections.Generic;

static long[] Smallest(long n)
{
    //Number of type long to array of type long[]
    List<long> numArray = n.ToString().ToArray().Select(x => Convert.ToInt64(x.ToString())).ToList();
    if (IsNotEqualElements(numArray))
    {
        //Count how many min digits are at the beginning of the number
        int countSkip = FindFirstDigitIndex(numArray);

        //The common case when at first finding min digit of number and then put it to the start of number
        (int indexMin, int indexMoveTo) minArrayParam = FindMinArray(numArray, countSkip);
        List<long> numArrayMin = MovingElement(numArray, minArrayParam.indexMin, countSkip);

        //The case like 1000000 when at first find max digit and put it to the end of number
        long max = numArray.Max();
        int indexMax = numArray.IndexOf(max);
        List<long> numArrayMax = MovingElement(numArray, indexMax, numArray.Count - 1);

        //Calculate numbers of long type for each case plus for case when at position 1 in number stays zero (ex. 10677)
        long numMin = Convert.ToInt64(string.Concat(numArrayMin));
        long numMax = Convert.ToInt64(string.Concat(numArrayMax));
        (int indexFirstDigit, long numAfter) numAfterMovingFirstDigit = MoveFirstDigit(numArray, countSkip);

        //Check what of the three cases gives the min number and return the corresponding result
        long minOfMin = Min(numMin, numMax, numAfterMovingFirstDigit.numAfter);
        if (minOfMin == numMin)
        {
            if (countSkip - minArrayParam.indexMin == -1)
                return new long[] { numMin, minArrayParam.indexMoveTo, minArrayParam.indexMin };
            else return new long[] { numMin, minArrayParam.indexMin, minArrayParam.indexMoveTo };
        }
        else if (minOfMin == numMax)
        {
            return new long[] { numMax, indexMax, numArray.Count - 1 };
        }
        else if (minOfMin == numAfterMovingFirstDigit.numAfter)
        {
            return new long[] { numAfterMovingFirstDigit.numAfter, countSkip, numAfterMovingFirstDigit.indexFirstDigit };
        }
    }
    return new long[] { n, 0, 0 };
}

//Count how many min digits are at the beginning of the number
static int FindFirstDigitIndex(List<long> array)
{
    long min = array.Min();
    int minIndex = array.IndexOf(min);
    int countSkip = 0;
    if (minIndex == 0)
    {
        countSkip++;
        for (int i = 1; i < array.Count; i++)
        {
            if (array[i] == min) countSkip++;
            else break;
        }
    }
    return countSkip;
}

//Find such min digit which moving will return the min number according to the task of kata (one abbility to move digit)
static (int, int) FindMinArray(List<long> numArray, int countSkip)
{
    List<long> temp = numArray.Skip(countSkip).ToList();
    long minTemp = temp.Min();
    int indexMin = temp.LastIndexOf(minTemp);
    //if there are close set of min digit in number (ex. 1070000) we should take the smallest index
    while ((indexMin - 1 >= 0) && 
        (temp.ElementAt(indexMin - 1) == minTemp))
    {
        --indexMin;
    }
    int indexMoveTo = 0;
    if (minTemp != numArray.Min())
        indexMoveTo = countSkip;
    return (indexMin + countSkip, indexMoveTo);    
}

//Moving the first digit to find the smallest number
static (int, long) MoveFirstDigit(List<long> numArray, int countSkip)
{
    long firstDigit = numArray[countSkip];
    long numInit = Convert.ToInt64(string.Concat(numArray));
    int index = countSkip + 1;
    List<long> temp = MovingElement(numArray, index - 1, index);
    long numAfterChange = Convert.ToInt64(string.Concat(temp));       
    while ((numAfterChange <= numInit) && (index + 1 < numArray.Count))
    {        
        index++;
        temp = MovingElement(temp, index - 1, index);        
        numInit = numAfterChange;
        numAfterChange = Convert.ToInt64(string.Concat(temp));        
    }
    return (--index, numInit);
}

//Check if all elements of array are equal
static bool IsNotEqualElements(List<long> array)
{
    bool result = true;
    long element = array[0];
    List<long> elementsEqual = array.Where(x => x == element).ToList();
    if (elementsEqual.Count == array.Count)
        result = false;
    return result;
}

//Moving elements in List
static List<long> MovingElement(List<long> array, int indexFrom, int indexTo)
{
    List<long> numArrayChange = array.ToList();
    long element = array.ElementAt(indexFrom);
    numArrayChange.RemoveAt(indexFrom);
    if (indexTo == array.Count - 1) numArrayChange.Add(element);
    else numArrayChange.Insert(indexTo, element);
    return numArrayChange;
}

//Choose min of three numbers
static long Min(long num1, long num2, long num3)
{
    if ((num1 <= num2) && (num1 <= num3)) return num1;
    else if ((num2 <= num1) && (num2 <= num3)) return num2;
    else if ((num3 <= num1) && (num3 <= num2)) return num3;
    return 0;
}

//testing
Console.WriteLine(String.Join(',', Smallest(285365)));
Console.WriteLine(String.Join(',', Smallest(197178875653625376)));
Console.WriteLine(String.Join(',', Smallest(199819884756)));
Console.WriteLine(String.Join(',', Smallest(100000)));
Console.WriteLine(String.Join(',', Smallest(43476174177360352)));
Console.WriteLine(String.Join(',', Smallest(7000142109589056)));

static long[] Smallest2(long n)
{
    long[] result = new long[] { n, 0, 0 };
    
    string str = n.ToString();

    for (int i = 0; i < str.Length; i++)
    {
        string element = str.Substring(i, 1);
        string temp = str.Remove(i, 1);

        for (int j = 0; j < str.Length; j++)
        {
            long min = Convert.ToInt64(temp.Insert(j, element));
            if (min < result[0]) 
            { 
                result[0] = min; 
                result[1] = i; 
                result[2] = j; 
            }
        }
    }
    return result;
}

Console.WriteLine(String.Join(',', Smallest2(285365)));
Console.WriteLine(String.Join(',', Smallest2(197178875653625376)));
Console.WriteLine(String.Join(',', Smallest2(199819884756)));
Console.WriteLine(String.Join(',', Smallest2(100000)));
Console.WriteLine(String.Join(',', Smallest2(43476174177360352)));
Console.WriteLine(String.Join(',', Smallest2(7000142109589056)));
