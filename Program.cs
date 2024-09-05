internal class Program
{
    private static void Main(string[] args)
    {
        //Skapar en sträng för att testa.
        string expression = "1 + (36/2) / (3 * 3) -2 + 12 - (20 / 5)";

        //Jag vill filtrera bort alla mellanslag för att när användaren skickar in är det inte säkert att mellanslag kommer med eller om användaren råkar ha med eller slarva med det.
        string filteredExpression = expression.Replace(" ", "");

        //Detta kommer vara main funktion som tar hand om det hela.
        var result = ExecuteCalculation(filteredExpression);

        Console.WriteLine("Resultat: " + result);
    }

    static string ExecuteCalculation(string expression)
    {
        //Jag skapade en char array som jag kan använda för att hitta start och stopp index
        char[] operators = {'-','/','+','*'};

        string finalExpression = "";
        //Först vill jag leta igenom strängen och hitta paranteserna och extrahera det som de håller och göra beräkningen och sedan ta bort paranteserna med innehållet och ersätta med svaret.
        //Jag tänker att använda mig av while loop som ska söka igenom strängen tills inga paranteser är kvar.
        while (expression.Contains("("))
        {
            //Dessa kommer att hjälpa mig att ta ut innehållet i strängen
            int openIndex = expression.LastIndexOf('(');
            int closeIndex = expression.IndexOf(')', openIndex);

            string filteredExpression = expression.Substring(openIndex + 1, closeIndex - openIndex - 1);

            //Nu när jag har extraherat det jag behöver, ska jag fixa funktion som ska omvandla innehållet till siffror och göra beräkningen.
            double result = ExpressionConverter(filteredExpression);

            //Efter den har räknat ut och retunerat svar måste jag ta original strängen och ersätta det som var i paranteserna med svaret.
            expression = expression.Substring(0, openIndex) + result.ToString() + expression.Substring(closeIndex + 1);

            finalExpression = expression;
        }
        // Nu när det funkar att räkna ut det som ligger inom parenteserna ska jag börja ta hand om multiplikationsoperatorer
        // Tänker att det är lättast att använda sig av en while loop här också.
        while (expression.Contains("/") || expression.Contains("*"))
        {
            // Här letar jag efter indexet av den första multiplikationsoperatorn som kommer
            int indexOfTheSymbol = expression.IndexOfAny(new char[] {'/', '*'});

            // Dessa två letar efter början och slutet av ekvationen jag behöver extrahera
            int leftStart = indexOfTheSymbol - 1;
            while(leftStart >= 0 && !operators.Contains(expression[leftStart])){
                leftStart--;
            }
            leftStart++;

            int rightEnd = indexOfTheSymbol + 1;
            while(rightEnd < expression.Length && !operators.Contains(expression[rightEnd])){
                rightEnd++;
            }

            // Här plockar jag ut den ekvationen jag behöver, och precis som i ovanstående loop gör jag beräkningen och ersätter ekvationen med svaret
            string filteredExpression = expression.Substring(leftStart, rightEnd - leftStart);

            double result = ExpressionConverter(filteredExpression);

            expression = expression.Substring(0, leftStart) + result.ToString() + expression.Substring(rightEnd);

            finalExpression = expression;
        }

        return finalExpression;
    }

    static double ExpressionConverter(string expression)
    {
        //Här ska jag ta isär strängen och lägga det i varsin behållare i en lista.
        List<string> expressionParts = new();
        string number = "";

        //Här går jag igenom en char åt gången för att sortera siffror och operatorer
        foreach (var item in expression)
        {
            //När den hittar nummer så lägger den in det i strängen.
            if (char.IsDigit(item))
            {
                number += item;
            }
            //Om den stöter på operator då lägger den först in nummer den har hittat i listan och sedan operator, efter det så nollställer den numbersträngen och fortsätter leta.
            else if(!char.IsDigit(item))
            {
                expressionParts.Add(number);
                expressionParts.Add(item.ToString());
                number = "";
            }
        }
        //Måste ha denna för att lägga in sista siffran i listan.
        if(!string.IsNullOrEmpty(number))
        {
            expressionParts.Add(number);
        }

        //Här så konventerar jag strängen till double genom att hämta dem från listan.
        double first = Convert.ToDouble(expressionParts[0]);
        string extractedOperator = expressionParts[1];
        double second = Convert.ToDouble(expressionParts[2]);

        return Calculate(first, extractedOperator, second);
    }

    static double Calculate(double first, string extractedOperator, double second)
    {
        //Här tänker jag att det är lättast och snyggast att göra en switch som går igenom alla operatorer och genomför beräkningen.
        switch (extractedOperator)
        {
            case "+":
                return first + second;
            case "-":
                return first - second;
            case "/":
                return first / second;
            case "*":
                return first * second;
            default:
                throw new Exception("Invalid operator");
        }
    }
}