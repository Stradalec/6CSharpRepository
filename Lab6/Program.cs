using System;
using System.ComponentModel;

namespace Lab6
{

    //Пользовательский класс исключений
    public class InvalidDepartmentException : System.Exception
    {
        public InvalidDepartmentException()
        : base() { }
        public InvalidDepartmentException(string message)
        : base(message) { }
        public InvalidDepartmentException
        (string message, System.Exception inner)
        : base(message, inner) { }

    }
    public class Matrix : IComparable
    {
        public double[,] PartsOfMatrix;
        public int RowsAndLines;
        private double _maximumElement = 0;
        private int _maximumElementRowIndex = 0;

        //Конструктор матрицы
        public Matrix()
        {
            Random random = new Random();
            RowsAndLines = random.Next(2, 5);
            PartsOfMatrix = new double[RowsAndLines, RowsAndLines];
            for (int rowIndex = 0; rowIndex < RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < RowsAndLines; ++columnIndex)
                {
                    PartsOfMatrix[rowIndex, columnIndex] = random.Next(0, 100);
                }
            }
        }

        // Другой конструктор матрицы (без него никак)
        public Matrix(int inputRowAndLineCount)
        {
            Random random = new Random();
            RowsAndLines = inputRowAndLineCount;
            PartsOfMatrix = new double[RowsAndLines, RowsAndLines];
            for (int rowIndex = 0; rowIndex < RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < RowsAndLines; ++columnIndex)
                {
                    PartsOfMatrix[rowIndex, columnIndex] = random.Next(0, 100);
                }
            }
        }

        //Метод клонирования матрицы
        public object Clone(int inputRowAndLineCount)
        {
            Matrix clonedMatrix = new Matrix(inputRowAndLineCount);
            clonedMatrix.RowsAndLines = this.RowsAndLines;

            for (int rowIndex = 0; rowIndex < clonedMatrix.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < clonedMatrix.RowsAndLines; ++columnIndex)
                {
                    clonedMatrix.PartsOfMatrix[rowIndex, columnIndex] = this.PartsOfMatrix[rowIndex, columnIndex];
                }
            }

            this.PartsOfMatrix[0, 0] = 0;

            return clonedMatrix;
        }

        // Приведение матрицы к диагональному виду
        public void MatrixToDiagonal(Matrix inputMatrix)
        {

            //Ищем главный элемент
            for (int rowIndex = 0; rowIndex < inputMatrix.RowsAndLines; ++rowIndex)
            {
                _maximumElement = Math.Abs(inputMatrix.PartsOfMatrix[rowIndex, rowIndex]);
                _maximumElementRowIndex = rowIndex;

                for (int searchMaximumElementIndex = rowIndex + 1; searchMaximumElementIndex < inputMatrix.RowsAndLines; ++searchMaximumElementIndex)
                {
                    if (Math.Abs(inputMatrix.PartsOfMatrix[searchMaximumElementIndex, rowIndex]) > _maximumElement)
                    {
                        _maximumElement = Math.Abs(inputMatrix.PartsOfMatrix[searchMaximumElementIndex, rowIndex]);
                        _maximumElementRowIndex = searchMaximumElementIndex;
                    }
                }
                //Перемещаем главный элемент на диагональ
                if (_maximumElementRowIndex != rowIndex)
                {
                    for (int columnIndex = rowIndex; columnIndex < inputMatrix.RowsAndLines; ++columnIndex)
                    {
                        double mainElement = inputMatrix.PartsOfMatrix[rowIndex, columnIndex];
                        inputMatrix.PartsOfMatrix[rowIndex, columnIndex] = inputMatrix.PartsOfMatrix[_maximumElementRowIndex, columnIndex];
                        inputMatrix.PartsOfMatrix[_maximumElementRowIndex, columnIndex] = mainElement;
                    }
                }
                // Непосредственно приведение матрицы к треугольному виду (ещё половина пути)
                for (int firstDiagonalIndex = rowIndex + 1; firstDiagonalIndex < inputMatrix.RowsAndLines; ++firstDiagonalIndex)
                {
                    double result = -inputMatrix.PartsOfMatrix[firstDiagonalIndex, rowIndex] / inputMatrix.PartsOfMatrix[rowIndex, rowIndex];
                    for (int secondDiagonalIndex = rowIndex; secondDiagonalIndex < inputMatrix.RowsAndLines; ++secondDiagonalIndex)
                    {
                        if (rowIndex == secondDiagonalIndex)
                        {
                            inputMatrix.PartsOfMatrix[firstDiagonalIndex, secondDiagonalIndex] = 0;
                        }
                        else
                        {
                            inputMatrix.PartsOfMatrix[firstDiagonalIndex, secondDiagonalIndex] += result * inputMatrix.PartsOfMatrix[rowIndex, secondDiagonalIndex];
                        }
                    }
                }
            }
            // А теперь - обратный ход
            for (int reverseCourseIndex = inputMatrix.RowsAndLines - 1; reverseCourseIndex > 0; --reverseCourseIndex)
            {
                for (int thirdDiagonalIndex = reverseCourseIndex - 1; thirdDiagonalIndex >= 0; --thirdDiagonalIndex)
                {
                    double result = -inputMatrix.PartsOfMatrix[thirdDiagonalIndex, reverseCourseIndex] / inputMatrix.PartsOfMatrix[reverseCourseIndex, reverseCourseIndex];
                    for (int fourthDiagonalIndex = inputMatrix.RowsAndLines - 1; fourthDiagonalIndex >= 0; --fourthDiagonalIndex)
                    {
                        inputMatrix.PartsOfMatrix[thirdDiagonalIndex, fourthDiagonalIndex] += result * inputMatrix.PartsOfMatrix[reverseCourseIndex, fourthDiagonalIndex];
                    }
                }
            }
            ShowMatrix(inputMatrix);
        }

        //Оператор сложения
        public static Matrix operator +(Matrix first, Matrix second)
        {
            Matrix resultMatrix = (Matrix)first.Clone(first.RowsAndLines);

            try
            {
                for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                    {
                        resultMatrix.PartsOfMatrix[rowIndex, columnIndex] = first.PartsOfMatrix[rowIndex, columnIndex] + second.PartsOfMatrix[rowIndex, columnIndex];
                    }
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
            }

            resultMatrix.ShowMatrix(resultMatrix);
            return resultMatrix;
        }

        //Оператор вычитания (в задании не было, но не удалять же, пусть живёт)
        public static Matrix operator -(Matrix first, Matrix second)
        {
            Matrix resultMatrix = (Matrix)first.Clone(first.RowsAndLines);

            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    resultMatrix.PartsOfMatrix[rowIndex, columnIndex] = first.PartsOfMatrix[rowIndex, columnIndex] - second.PartsOfMatrix[rowIndex, columnIndex];
                }
            }
            resultMatrix.ShowMatrix(resultMatrix);
            return resultMatrix;
        }

        //Оператор умножения
        public static Matrix operator *(Matrix first, Matrix second)
        {
            Matrix resultMatrix = (Matrix)first.Clone(first.RowsAndLines);

            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                Console.Write("\n");
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    for (int innerIndex = 0; innerIndex < first.RowsAndLines; ++innerIndex)
                    {
                        resultMatrix.PartsOfMatrix[rowIndex, columnIndex] += first.PartsOfMatrix[rowIndex, innerIndex] * second.PartsOfMatrix[innerIndex, columnIndex];
                    }

                    Console.Write(resultMatrix.PartsOfMatrix[rowIndex, columnIndex] + " ");
                    resultMatrix.PartsOfMatrix[rowIndex, columnIndex] = 0;
                }

            }
            return resultMatrix;
        }

        //Оператор обратной матрицы (работает, но у него своё понимание чисел)
        public static Matrix operator +(Matrix inputMatrix)
        {
            Matrix resultMatrix = (Matrix)inputMatrix.Clone(inputMatrix.RowsAndLines);
            int temporaryLength = inputMatrix.PartsOfMatrix.GetLength(0);
            double[,] augmentedMatrix = new double[temporaryLength, temporaryLength * 2];

            //К исходной матрице прибавляем единичную справа
            for (int rowIndex = 0; rowIndex < inputMatrix.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < inputMatrix.RowsAndLines; ++columnIndex)
                {
                    augmentedMatrix[rowIndex, columnIndex] = inputMatrix.PartsOfMatrix[rowIndex, columnIndex];
                    augmentedMatrix[rowIndex, columnIndex + temporaryLength] = (rowIndex == columnIndex) ? 1 : 0; // если элемент единичной матрицы на её диагонали, то 1, иначе 2
                }
            }

            // Приводим левую часть к единичной
            for (int leftRowIndex = 0; leftRowIndex < temporaryLength; ++leftRowIndex)
            {
                double mainElement = augmentedMatrix[leftRowIndex, leftRowIndex];

                for (int leftColumnIndex = 0; leftColumnIndex < 2 * temporaryLength; ++leftColumnIndex)
                {
                    augmentedMatrix[leftRowIndex, leftColumnIndex] /= mainElement;
                }

                for (int reverseIndex = 0; reverseIndex < temporaryLength; ++reverseIndex)
                {
                    if (reverseIndex != leftRowIndex)
                    {
                        double coefficient = augmentedMatrix[reverseIndex, leftRowIndex];

                        for (int anotherReverseIndex = 0; anotherReverseIndex < 2 * temporaryLength; ++anotherReverseIndex)
                        {
                            augmentedMatrix[reverseIndex, anotherReverseIndex] -= coefficient * augmentedMatrix[leftRowIndex, anotherReverseIndex];
                        }

                    }
                }
            }
            //Перезаписываем результат
            for (int outputRowIndex = 0; outputRowIndex < inputMatrix.RowsAndLines; ++outputRowIndex)
            {
                for (int outputColumnIndex = 0; outputColumnIndex < inputMatrix.RowsAndLines; ++outputColumnIndex)
                {
                    resultMatrix.PartsOfMatrix[outputRowIndex, outputColumnIndex] = augmentedMatrix[outputRowIndex, outputColumnIndex + temporaryLength];
                }
            }

            resultMatrix.ShowMatrix(resultMatrix);
            return resultMatrix;
        }

        //Метод вывода матрицы
        public void ShowMatrix(Matrix inputMatrix)
        {
            for (int rowIndex = 0; rowIndex < inputMatrix.RowsAndLines; ++rowIndex)
            {
                Console.Write("\n");
                for (int columnIndex = 0; columnIndex < inputMatrix.RowsAndLines; ++columnIndex)
                {
                    Console.Write(PartsOfMatrix[rowIndex, columnIndex].ToString("0.00 "));
                }
            }
            Console.Write("\n");
        }

        //Метод расчёта определителя
        public static double operator !(Matrix inputMatrix)
        {
            double determinant = 0;

            switch (inputMatrix.RowsAndLines)
            {
                case 2:
                    determinant = inputMatrix.PartsOfMatrix[0, 0] * inputMatrix.PartsOfMatrix[1, 1] - inputMatrix.PartsOfMatrix[0, 1] * inputMatrix.PartsOfMatrix[1, 0];
                    break;
                case 3:
                    determinant = (inputMatrix.PartsOfMatrix[0, 0] * inputMatrix.PartsOfMatrix[1, 1] * inputMatrix.PartsOfMatrix[2, 2] +
                                  inputMatrix.PartsOfMatrix[0, 2] * inputMatrix.PartsOfMatrix[1, 0] * inputMatrix.PartsOfMatrix[2, 1] +
                                  inputMatrix.PartsOfMatrix[0, 1] * inputMatrix.PartsOfMatrix[1, 2] * inputMatrix.PartsOfMatrix[2, 0]) -
                                  (inputMatrix.PartsOfMatrix[0, 2] * inputMatrix.PartsOfMatrix[1, 1] * inputMatrix.PartsOfMatrix[2, 0] +
                                  inputMatrix.PartsOfMatrix[0, 1] * inputMatrix.PartsOfMatrix[1, 0] * inputMatrix.PartsOfMatrix[2, 2] +
                                  inputMatrix.PartsOfMatrix[0, 0] * inputMatrix.PartsOfMatrix[1, 2] * inputMatrix.PartsOfMatrix[2, 1]);
                    break;
                default:
                    for (int determinantIndex = 0; determinantIndex < inputMatrix.RowsAndLines; ++determinantIndex)
                    {
                        determinant += Math.Pow(-1, determinantIndex) * inputMatrix.PartsOfMatrix[0, determinantIndex] * !(CreateSmallMatrix(inputMatrix, 0, determinantIndex));
                    }
                    break;
            }
            return determinant;
        }

        public static Matrix CreateSmallMatrix(Matrix inputMatrix, int deleteRow, int deleteColumn)
        {
            int length = inputMatrix.PartsOfMatrix.GetLength(0);
            Matrix smallMatrix = new Matrix(length - 1);
            int smallRow = -1;
            int smallColumn = -1;

            for (int firstDeleteIndex = 0; firstDeleteIndex < length; ++firstDeleteIndex)
            {
                if (firstDeleteIndex == deleteRow)
                {
                    continue;
                }
                ++smallRow;

                for (int secondDeleteIndex = 0; secondDeleteIndex < length; ++secondDeleteIndex)
                {
                    if (secondDeleteIndex == deleteColumn)
                    {
                        continue;
                    }
                    smallMatrix.PartsOfMatrix[smallRow, ++smallColumn] = inputMatrix.PartsOfMatrix[firstDeleteIndex, secondDeleteIndex];
                }
            }
            return smallMatrix;
        }

        //Оператор сравнения
        public static bool operator ==(Matrix first, Matrix second)
        {
            byte check = 0;
            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    if (first.PartsOfMatrix[rowIndex, columnIndex] != second.PartsOfMatrix[rowIndex, columnIndex])
                    {
                        ++check;
                    };
                }
            }
            if (check == 0)
            {
                return check == 0;
            }
            else
            {
                return check == 1;
            }
        }

        //Оператор сравнения (ещё)
        public static bool operator >(Matrix first, Matrix second)
        {
            double result = 0;
            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    result = result + first.PartsOfMatrix[rowIndex, columnIndex] * first.PartsOfMatrix[rowIndex, columnIndex] - second.PartsOfMatrix[rowIndex, columnIndex] * second.PartsOfMatrix[rowIndex, columnIndex];
                }
            }
            return result > 0;
        }

        //Оператор сравнения (ещё)
        public static bool operator <(Matrix first, Matrix second)
        {
            double result = 0;
            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    result = result + first.PartsOfMatrix[rowIndex, columnIndex] * first.PartsOfMatrix[rowIndex, columnIndex] - second.PartsOfMatrix[rowIndex, columnIndex] * second.PartsOfMatrix[rowIndex, columnIndex];
                }
            }
            return result < 0;
        }

        //Оператор сравнения (ещё)
        public static bool operator !=(Matrix first, Matrix second)
        {
            byte check = 0;
            for (int rowIndex = 0; rowIndex < first.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < first.RowsAndLines; ++columnIndex)
                {
                    if (first.PartsOfMatrix[rowIndex, columnIndex] != second.PartsOfMatrix[rowIndex, columnIndex])
                    {
                        ++check;
                    };
                }
            }
            if (check != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Оператор сравнения (ещё)
        public static bool operator >=(Matrix first, Matrix second)
        {
            return !(first < second);
        }

        //Оператор сравнения (ещё)
        public static bool operator <=(Matrix first, Matrix second)
        {
            return !(first > second);
        }

        //Оператор сравнения (ещё)
        public override bool Equals(object myObject)
        {
            bool result = false;
            if (myObject is Matrix)
            {
                var parameter = myObject as Matrix;
                if (parameter.RowsAndLines == this.RowsAndLines)
                {
                    result = true;
                }
            }
            return result;
        }

        //Взять код элемента
        public override int GetHashCode()
        {
            return (int)this.RowsAndLines;
        }

        //Неявное преобразования
        public static implicit operator double[,](Matrix inputMatrix)
        {
            return inputMatrix.PartsOfMatrix;
        }

        //Явное преобразования
        public static explicit operator double(Matrix inputMatrix)
        {
            return inputMatrix.RowsAndLines;
        }

        //Оператор True
        public static bool operator true(Matrix inputMatrix)
        {
            return inputMatrix.RowsAndLines != 1;
        }

        //Оператор False
        public static bool operator false(Matrix inputMatrix)
        {
            return inputMatrix.RowsAndLines == 1;
        }

        //Оператор CompareTo
        int IComparable.CompareTo(object myObject)
        {
            if (myObject is Matrix)
            {
                var parameter = myObject as Matrix;
                if (parameter.RowsAndLines > this.RowsAndLines)
                {
                    return -1;
                }
                if (parameter.RowsAndLines == this.RowsAndLines)
                {
                    return 0;
                }
                if (parameter.RowsAndLines < this.RowsAndLines)
                {
                    return 1;
                }
            }
            return -1;
        }

    }

    //Класс расширяющихся методов
    public static class ExtensionsMetods
    {
        private static double traceOfMatrix = 0;
        private static Matrix resultMatrix = new Matrix();

        //След матрицы
        public static double MatrixTrace(this Matrix inputMatrix)
        {
            for (int rowIndex = 0; rowIndex < inputMatrix.RowsAndLines; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < inputMatrix.RowsAndLines; ++columnIndex)
                {
                    if (inputMatrix.PartsOfMatrix[rowIndex, columnIndex] == inputMatrix.PartsOfMatrix[columnIndex, rowIndex])
                    {
                        traceOfMatrix += inputMatrix.PartsOfMatrix[rowIndex, columnIndex];
                    }
                }
            }
            return traceOfMatrix;
        }

        //Транспонирование
        public static Matrix MatrixTranspose(this Matrix inputMatrix)
        {
            for (int rowIndex = 0; rowIndex < inputMatrix.RowsAndLines; ++rowIndex)
            {
                Console.Write("\n");
                for (int columnIndex = 0; columnIndex < inputMatrix.RowsAndLines; ++columnIndex)
                {
                    Console.Write(inputMatrix.PartsOfMatrix[columnIndex, rowIndex] + " ");
                }
            }
            return resultMatrix;
        }
    }

    //Предок всех событий
    public abstract class AncestorOfEvents
    {
        public string EventType { get; set; }
        public virtual void Call(Matrix inputMatrix)
        {

        }
    }

    //Событие сложения
    class SummEvent : AncestorOfEvents
    {
        public SummEvent()
        {
            EventType = "Summ";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((inputMatrix + inputMatrix).ToString() + "\n"); ;
        }
    }

    //Событие вычитания
    class DifferenceEvent : AncestorOfEvents
    {
        public DifferenceEvent()
        {
            EventType = "Difference";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((inputMatrix - inputMatrix).ToString() + "\n");
        }
    }

    //Событие сравнения
    class CompareEvent : AncestorOfEvents
    {
        public CompareEvent()
        {
            EventType = "Compare";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((inputMatrix == inputMatrix).ToString() + "\n");
        }
    }

    //Событие умножения
    class MultiplicationEvent : AncestorOfEvents
    {
        public MultiplicationEvent()
        {
            EventType = "Multiplication";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((inputMatrix * inputMatrix).ToString() + "\n");
        }
    }

    //Событие обратной матрицы
    class ReverseEvent : AncestorOfEvents
    {
        public ReverseEvent()
        {
            EventType = "Reverse";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((+inputMatrix).ToString() + "\n");
        }
    }

    //Событие определителя
    class DeterminantEvent : AncestorOfEvents
    {
        public DeterminantEvent()
        {
            EventType = "Determinant";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine((!inputMatrix).ToString() + "\n");
        }
    }

    //Событие следа
    class TraceEvent : AncestorOfEvents
    {
        public TraceEvent()
        {
            EventType = "Trace";
        }
        public override void Call(Matrix inputMatrix)
        {
            double trace = inputMatrix.MatrixTrace();
            Console.WriteLine(trace);
        }
    }

    //Событие транспонирования
    class TransposeEvent : AncestorOfEvents
    {
        public TransposeEvent()
        {
            EventType = "Transpose";
        }
        public override void Call(Matrix inputMatrix)
        {
            inputMatrix = inputMatrix.MatrixTranspose();
        }
    }

    //Событие диагональной матрицы
    class DiagonalEvent : AncestorOfEvents
    {
        public DiagonalEvent()
        {
            EventType = "Diagonal";
        }
        public override void Call(Matrix inputMatrix)
        {
            Console.WriteLine("Hello, I am anonymous method");
            MatrixToDiag anonymousMethod = delegate (Matrix myNewMatrix) {
                inputMatrix.MatrixToDiagonal(inputMatrix);
            };
            anonymousMethod.Invoke(inputMatrix);
        }
    }

    class LonelyEvent : AncestorOfEvents
    {
        public LonelyEvent()
        {
            EventType = "Lonely, no operation";
        }
    }

    // Предок всех обработчиков
    public abstract class AncestorOfHandler
    {
        protected AncestorOfEvents InnerEvent { get; set; }
        protected AncestorOfHandler NextEvent { get; set; }

        public AncestorOfHandler()
        {
            NextEvent = null;
        }
        public virtual void Handle(AncestorOfEvents inputEvent, Matrix inputMatrix)
        {
            if (InnerEvent.EventType == inputEvent.EventType)
            {
                inputEvent.Call(inputMatrix);
                Console.WriteLine("{0} exist and executed", InnerEvent.EventType);
            }
            else
            {
                Console.WriteLine("Trying to find event type, sending to NextEvent....");
                if (NextEvent != null)
                {
                    NextEvent.Handle(inputEvent, inputMatrix);
                }
                else
                {
                    Console.WriteLine("Event does not exist");
                }
            }
        }

        protected void SetNextHandler(AncestorOfHandler newHanlder)
        {
            NextEvent = newHanlder;
        }
    }

    //Обработчики
    class SummHandler : AncestorOfHandler
    {
        public SummHandler()
        {
            InnerEvent = new SummEvent();
            NextEvent = new DifferenceHandler();
        }
    }

    class DifferenceHandler : AncestorOfHandler
    {
        public DifferenceHandler()
        {
            InnerEvent = new DifferenceEvent();
            NextEvent = new CompareHandler();
        }
    }

    class CompareHandler : AncestorOfHandler
    {
        public CompareHandler()
        {
            InnerEvent = new CompareEvent();
            NextEvent = new MultiplicationHandler();
        }
    }
    class MultiplicationHandler : AncestorOfHandler
    {
        public MultiplicationHandler()
        {
            InnerEvent = new MultiplicationEvent();
            NextEvent = new ReverseHandler();
        }
    }
    class ReverseHandler : AncestorOfHandler
    {
        public ReverseHandler()
        {
            InnerEvent = new ReverseEvent();
            NextEvent = new DeterminantHandler();
        }
    }

    class DeterminantHandler : AncestorOfHandler
    {
        public DeterminantHandler()
        {
            InnerEvent = new DeterminantEvent();
            NextEvent = new TraceHandler();
        }
    }
    class TraceHandler : AncestorOfHandler
    {
        public TraceHandler()
        {
            InnerEvent = new TraceEvent();
            NextEvent = new TransposeHandler();
        }
    }

    class TransposeHandler : AncestorOfHandler
    {
        public TransposeHandler()
        {
            InnerEvent = new TransposeEvent();
            NextEvent = new DiagonalHandler();
        }
    }
    class DiagonalHandler : AncestorOfHandler
    {
        public DiagonalHandler()
        {
            InnerEvent = new DiagonalEvent();
            NextEvent = null;
        }
    }

    //Класс "Цепь подчинения" (честно украдено из лекции)
    public class ChainApplication
    {
        private AncestorOfHandler _eventHandler;
        private int _typeOfOperation;
        public ChainApplication(Matrix inputMatrix, int inputType)
        {
            _eventHandler = new SummHandler();
            _typeOfOperation = inputType;

        }
        public void Run(int eventCount, Matrix inputMatrix)
        {
            for (int eventIndex = 0; eventIndex < eventCount; ++eventIndex)
            {
                handleEvent(GenerateRandomEvent(inputMatrix), inputMatrix);
            }
        }
        public void handleEvent(AncestorOfEvents inputEvent, Matrix inputMatrix)
        {
            _eventHandler.Handle(inputEvent, inputMatrix);
        }

        private AncestorOfEvents GenerateRandomEvent(Matrix inputMatrix)
        {
            AncestorOfEvents result;
            switch (_typeOfOperation)
            {
                case 0:
                    result = new SummEvent();
                    break;
                case 1:
                    result = new DifferenceEvent();
                    break;
                case 2:
                    result = new CompareEvent();
                    break;
                case 3:
                    result = new MultiplicationEvent();
                    break;
                case 4:
                    result = new ReverseEvent();
                    break;
                case 5:
                    result = new DeterminantEvent();
                    break;
                case 6:
                    result = new TraceEvent();
                    break;
                case 7:
                    result = new TransposeEvent();
                    break;
                case 8:
                    result = new DiagonalEvent();
                    break;
                default:
                    result = new LonelyEvent();
                    break;
            }
            Console.WriteLine("Chosen event: {0}", result.EventType);
            return result;
        }
    }

    //Делегат анонимной функции
    public delegate void MatrixToDiag(Matrix inputMatrix);

    internal class Program
    {

        //Основная часть
        static void Main(string[] args)
        {
            Matrix myMatrix = new Matrix();
            int choice = 0;
            ChainApplication eventSelector = new ChainApplication(myMatrix, choice);
            Matrix myCloneMatrix = (Matrix)myMatrix.Clone(myMatrix.RowsAndLines);
            Matrix resultMatrix = (Matrix)myMatrix.Clone(myMatrix.RowsAndLines);
            bool isRunning = true;
            bool isBeingHere = true;
            char canNotWorkwithoutChar;

            //Показываем пользователю матрицы
            myMatrix.ShowMatrix(myMatrix);
            myCloneMatrix.ShowMatrix(myCloneMatrix);

            //Основной цикл работы основной части
            while (isRunning)
            {
                Console.WriteLine("choice your operation: 0 = summ, 1 = difference, 2 = compare, 3 = multiplication, " +
                          "4 = reverse matrix, 5 = determinant, 6 = trace, 7 = transpOse, 8 = turn to diagonal");
                choice = Convert.ToInt32(Console.ReadLine());

                eventSelector = new ChainApplication(myMatrix, choice);
                eventSelector.Run(1, myMatrix);

                Console.WriteLine("Enough? y/n");
                canNotWorkwithoutChar = Convert.ToChar(Console.ReadLine());
                if (canNotWorkwithoutChar == 'y' || canNotWorkwithoutChar == 'Y')
                {
                    Console.WriteLine("Okay. Press any button to exit completely");
                    isBeingHere = false;
                    isRunning = false;
                }
                else
                {
                    myMatrix = new Matrix();
                    Console.WriteLine("Your new matrix:");
                    myMatrix.ShowMatrix(myMatrix);
                    continue;
                }
            }

            Console.ReadKey();
        }

    }
}
