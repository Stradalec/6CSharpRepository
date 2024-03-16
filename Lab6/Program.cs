using System;

namespace Lab6 {

  //Пользовательский класс исключений
  public class InvalidDepartmentException : System.Exception {
    public InvalidDepartmentException()
    : base() { }
    public InvalidDepartmentException(string message)
    : base(message) { }
    public InvalidDepartmentException
    (string message, System.Exception inner)
    : base(message, inner) { }

  }
  public class Matrix : IComparable {
    public double[,] InnerPartOfMatrix;
    public int RowAndLineCount;
    private double maxElement = 0;
    private int rowOfMaximumElement = 0;

    //Конструктор матрицы
    public Matrix() {
      Random random = new Random();
      RowAndLineCount = random.Next(2, 5);
      InnerPartOfMatrix = new double[RowAndLineCount, RowAndLineCount];
      for (int rowIndex = 0; rowIndex < RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < RowAndLineCount; ++columnIndex) {
          InnerPartOfMatrix[rowIndex, columnIndex] = random.Next(0, 100);
        }
      }
    }

    // Другой конструктор матрицы (без него никак)
    public Matrix(int inputRowAndLineCount) {
      Random random = new Random();
      RowAndLineCount = inputRowAndLineCount;
      InnerPartOfMatrix = new double[RowAndLineCount, RowAndLineCount];
      for (int rowIndex = 0; rowIndex < RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < RowAndLineCount; ++columnIndex) {
          InnerPartOfMatrix[rowIndex, columnIndex] = random.Next(0, 100);
        }
      }
    }

    //Метод клонирования матрицы
    public object Clone(int inputRowAndLineCount) {
      Matrix clonedRowCount = new Matrix(inputRowAndLineCount);
      clonedRowCount.RowAndLineCount = this.RowAndLineCount;

      for (int rowIndex = 0; rowIndex < clonedRowCount.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < clonedRowCount.RowAndLineCount; ++columnIndex) {
          clonedRowCount.InnerPartOfMatrix[rowIndex, columnIndex] = this.InnerPartOfMatrix[rowIndex, columnIndex];
        }
      }

      this.InnerPartOfMatrix[0, 0] = 0;

      return clonedRowCount;
    }

    // Приведение матрицы к диагональному виду
    public void MatrixToDiagonal(Matrix inputMatrix) {

      //Ищем главный элемент
      for (int rowIndex = 0; rowIndex < inputMatrix.RowAndLineCount; ++rowIndex) {
        maxElement = Math.Abs(inputMatrix.InnerPartOfMatrix[rowIndex, rowIndex]);
        rowOfMaximumElement = rowIndex;

        for (int searchIndex = rowIndex + 1; searchIndex < inputMatrix.RowAndLineCount; ++searchIndex) {
          if (Math.Abs(inputMatrix.InnerPartOfMatrix[searchIndex, rowIndex]) > maxElement) {
            maxElement = Math.Abs(inputMatrix.InnerPartOfMatrix[searchIndex, rowIndex]);
            rowOfMaximumElement = searchIndex;
          }
        }
        //Перемещаем главный элемент на диагональ
        if (rowOfMaximumElement != rowIndex) {
          for (int columnIndex = rowIndex; columnIndex < inputMatrix.RowAndLineCount; ++columnIndex) {
            double secondTemporary = inputMatrix.InnerPartOfMatrix[rowIndex, columnIndex];
            inputMatrix.InnerPartOfMatrix[rowIndex, columnIndex] = inputMatrix.InnerPartOfMatrix[rowOfMaximumElement, columnIndex];
            inputMatrix.InnerPartOfMatrix[rowOfMaximumElement, columnIndex] = secondTemporary;
          }
        }
        // Непосредственно приведение матрицы к треугольному виду (ещё половина пути)
        for (int firstDiagIndex = rowIndex + 1; firstDiagIndex < inputMatrix.RowAndLineCount; ++firstDiagIndex) {
          double result = -inputMatrix.InnerPartOfMatrix[firstDiagIndex, rowIndex] / inputMatrix.InnerPartOfMatrix[rowIndex, rowIndex];
          for (int secondDiagIndex = rowIndex; secondDiagIndex < inputMatrix.RowAndLineCount; ++secondDiagIndex) {
            if (rowIndex == secondDiagIndex) {
              inputMatrix.InnerPartOfMatrix[firstDiagIndex, secondDiagIndex] = 0;
            } else {
              inputMatrix.InnerPartOfMatrix[firstDiagIndex, secondDiagIndex] += result * inputMatrix.InnerPartOfMatrix[rowIndex, secondDiagIndex];
            }
          }
        }
      }
      // А теперь - обратный ход
      for (int anotherSearchIndex = inputMatrix.RowAndLineCount - 1; anotherSearchIndex > 0; --anotherSearchIndex) {
        for (int thirdDiagIndex = anotherSearchIndex - 1; thirdDiagIndex >= 0; --thirdDiagIndex) {
          double result = -inputMatrix.InnerPartOfMatrix[thirdDiagIndex, anotherSearchIndex] / inputMatrix.InnerPartOfMatrix[anotherSearchIndex, anotherSearchIndex];
          for (int fourthDiagIndex = inputMatrix.RowAndLineCount - 1; fourthDiagIndex >= 0; --fourthDiagIndex) {
            inputMatrix.InnerPartOfMatrix[thirdDiagIndex, fourthDiagIndex] += result * inputMatrix.InnerPartOfMatrix[anotherSearchIndex, fourthDiagIndex];
          }
        }
      }
      ShowMatrix(inputMatrix);
    }

    //Оператор сложения
    public static Matrix operator +(Matrix first, Matrix second) {
      Matrix resultMatrix = (Matrix)first.Clone(first.RowAndLineCount);

      try {
        for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
          for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
            resultMatrix.InnerPartOfMatrix[rowIndex, columnIndex] = first.InnerPartOfMatrix[rowIndex, columnIndex] + second.InnerPartOfMatrix[rowIndex, columnIndex];
          }
        }
      } catch (IndexOutOfRangeException exception) {
        Console.WriteLine(exception.Message);
      }

      resultMatrix.ShowMatrix(resultMatrix);
      return resultMatrix;
    }

    //Оператор вычитания (в задании не было, но не удалять же, пусть живёт)
    public static Matrix operator -(Matrix first, Matrix second) {
      Matrix resultMatrix = (Matrix)first.Clone(first.RowAndLineCount);

      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          resultMatrix.InnerPartOfMatrix[rowIndex, columnIndex] = first.InnerPartOfMatrix[rowIndex, columnIndex] - second.InnerPartOfMatrix[rowIndex, columnIndex];
        }
      }
      resultMatrix.ShowMatrix(resultMatrix);
      return resultMatrix;
    }

    //Оператор умножения
    public static Matrix operator *(Matrix first, Matrix second) {
      Matrix resultMatrix = (Matrix)first.Clone(first.RowAndLineCount);

      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        Console.Write("\n");
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          for (int innerIndex = 0; innerIndex < first.RowAndLineCount; ++innerIndex) {
            resultMatrix.InnerPartOfMatrix[rowIndex, columnIndex] += first.InnerPartOfMatrix[rowIndex, innerIndex] * second.InnerPartOfMatrix[innerIndex, columnIndex];
          }

          Console.Write(resultMatrix.InnerPartOfMatrix[rowIndex, columnIndex] + " ");
          resultMatrix.InnerPartOfMatrix[rowIndex, columnIndex] = 0;
        }

      }
      return resultMatrix;
    }

    //Оператор обратной матрицы (работает, но у него своё понимание чисел)
    public static Matrix operator +(Matrix inputMatrix) {
      Matrix resultMatrix = (Matrix)inputMatrix.Clone(inputMatrix.RowAndLineCount);
      int temporaryLength = inputMatrix.InnerPartOfMatrix.GetLength(0);
      double[,] augmentedMatrix = new double[temporaryLength, temporaryLength * 2];

      //К исходной матрице прибавляем единичную справа
      for (int rowIndex = 0; rowIndex < inputMatrix.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < inputMatrix.RowAndLineCount; ++columnIndex) {
          augmentedMatrix[rowIndex, columnIndex] = inputMatrix.InnerPartOfMatrix[rowIndex, columnIndex];
          augmentedMatrix[rowIndex, columnIndex + temporaryLength] = (rowIndex == columnIndex) ? 1 : 0; // если элемент единичной матрицы на её диагонали, то 1, иначе 2
        }
      }

      // Приводим левую часть к единичной
      for (int leftRowIndex = 0; leftRowIndex < temporaryLength; ++leftRowIndex) {
        double secondTemporary = augmentedMatrix[leftRowIndex, leftRowIndex];

        for (int leftColumnIndex = 0; leftColumnIndex < 2 * temporaryLength; ++leftColumnIndex) {
          augmentedMatrix[leftRowIndex, leftColumnIndex] /= secondTemporary;
        }

        for (int reverseIndex = 0; reverseIndex < temporaryLength; ++reverseIndex) {
          if (reverseIndex != leftRowIndex) {
            double coefficient = augmentedMatrix[reverseIndex, leftRowIndex];

            for (int anotherReverseIndex = 0; anotherReverseIndex < 2 * temporaryLength; ++anotherReverseIndex) {
              augmentedMatrix[reverseIndex, anotherReverseIndex] -= coefficient * augmentedMatrix[leftRowIndex, anotherReverseIndex];
            }

          }
        }
      }
      //Перезаписываем результат
      for (int outputRowIndex = 0; outputRowIndex < inputMatrix.RowAndLineCount; ++outputRowIndex) {
        for (int outputColumnIndex = 0; outputColumnIndex < inputMatrix.RowAndLineCount; ++outputColumnIndex) {
          resultMatrix.InnerPartOfMatrix[outputRowIndex, outputColumnIndex] = augmentedMatrix[outputRowIndex, outputColumnIndex + temporaryLength];
        }
      }

      resultMatrix.ShowMatrix(resultMatrix);
      return resultMatrix;
    }

    //Метод вывода матрицы
    public void ShowMatrix(Matrix inputMatrix) {
      for (int rowIndex = 0; rowIndex < inputMatrix.RowAndLineCount; ++rowIndex) {
        Console.Write("\n");
        for (int columnIndex = 0; columnIndex < inputMatrix.RowAndLineCount; ++columnIndex) {
          Console.Write(InnerPartOfMatrix[rowIndex, columnIndex].ToString("0.00 "));
        }
      }
      Console.Write("\n");
    }

    //Метод расчёта определителя
    public static double operator !(Matrix inputMatrix) {
      double determinant = 0;

      switch (inputMatrix.RowAndLineCount) {
        case 2:
          determinant = inputMatrix.InnerPartOfMatrix[0, 0] * inputMatrix.InnerPartOfMatrix[1, 1] - inputMatrix.InnerPartOfMatrix[0, 1] * inputMatrix.InnerPartOfMatrix[1, 0];
          break;
        case 3:
          determinant = (inputMatrix.InnerPartOfMatrix[0, 0] * inputMatrix.InnerPartOfMatrix[1, 1] * inputMatrix.InnerPartOfMatrix[2, 2] +
                        inputMatrix.InnerPartOfMatrix[0, 2] * inputMatrix.InnerPartOfMatrix[1, 0] * inputMatrix.InnerPartOfMatrix[2, 1] +
                        inputMatrix.InnerPartOfMatrix[0, 1] * inputMatrix.InnerPartOfMatrix[1, 2] * inputMatrix.InnerPartOfMatrix[2, 0]) -
                        (inputMatrix.InnerPartOfMatrix[0, 2] * inputMatrix.InnerPartOfMatrix[1, 1] * inputMatrix.InnerPartOfMatrix[2, 0] +
                        inputMatrix.InnerPartOfMatrix[0, 1] * inputMatrix.InnerPartOfMatrix[1, 0] * inputMatrix.InnerPartOfMatrix[2, 2] +
                        inputMatrix.InnerPartOfMatrix[0, 0] * inputMatrix.InnerPartOfMatrix[1, 2] * inputMatrix.InnerPartOfMatrix[2, 1]);
          break;
        default:
          for (int determinantIndex = 0; determinantIndex < inputMatrix.RowAndLineCount; ++determinantIndex) {
            determinant += Math.Pow(-1, determinantIndex) * inputMatrix.InnerPartOfMatrix[0, determinantIndex] * !(CreateSmallMatrix(inputMatrix, 0, determinantIndex));
          }
          break;
      }
      return determinant;
    }

    public static Matrix CreateSmallMatrix(Matrix inputMatrix, int deleteRow, int deleteColumn) {
      int length = inputMatrix.InnerPartOfMatrix.GetLength(0);
      Matrix smallMatrix = new Matrix(length - 1);
      int smallRow = -1;
      int smallColumn = -1;

      for (int firstDeleteIndex = 0; firstDeleteIndex < length; ++firstDeleteIndex) {
        if (firstDeleteIndex == deleteRow) {
          continue;
        }
        ++smallRow;

        for (int secondDeleteIndex = 0; secondDeleteIndex < length; ++secondDeleteIndex) {
          if (secondDeleteIndex == deleteColumn) {
            continue;
          }
          smallMatrix.InnerPartOfMatrix[smallRow, ++smallColumn] = inputMatrix.InnerPartOfMatrix[firstDeleteIndex, secondDeleteIndex];
        }
      }
      return smallMatrix;
    }

    //Оператор сравнения
    public static bool operator ==(Matrix first, Matrix second) {
      byte check = 0;
      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          if (first.InnerPartOfMatrix[rowIndex, columnIndex] != second.InnerPartOfMatrix[rowIndex, columnIndex]) {
            ++check;
          };
        }
      }
      if (check == 0) {
        return true;
      } else {
        return false;
      }
    }

    //Оператор сравнения (ещё)
    public static bool operator >(Matrix first, Matrix second) {
      double result = 0;
      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          result = result + first.InnerPartOfMatrix[rowIndex, columnIndex] * first.InnerPartOfMatrix[rowIndex, columnIndex] - second.InnerPartOfMatrix[rowIndex, columnIndex] * second.InnerPartOfMatrix[rowIndex, columnIndex];
        }
      }
      return result > 0;
    }

    //Оператор сравнения (ещё)
    public static bool operator <(Matrix first, Matrix second) {
      double result = 0;
      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          result = result + first.InnerPartOfMatrix[rowIndex, columnIndex] * first.InnerPartOfMatrix[rowIndex, columnIndex] - second.InnerPartOfMatrix[rowIndex, columnIndex] * second.InnerPartOfMatrix[rowIndex, columnIndex];
        }
      }
      return result < 0;
    }

    //Оператор сравнения (ещё)
    public static bool operator !=(Matrix first, Matrix second) {
      byte check = 0;
      for (int rowIndex = 0; rowIndex < first.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < first.RowAndLineCount; ++columnIndex) {
          if (first.InnerPartOfMatrix[rowIndex, columnIndex] != second.InnerPartOfMatrix[rowIndex, columnIndex]) {
            ++check;
          };
        }
      }
      if (check != 0) {
        return true;
      } else {
        return false;
      }
    }

    //Оператор сравнения (ещё)
    public static bool operator >=(Matrix first, Matrix second) {
      return !(first < second);
    }

    //Оператор сравнения (ещё)
    public static bool operator <=(Matrix first, Matrix second) {
      return !(first > second);
    }

    //Оператор сравнения (ещё)
    public override bool Equals(object myObject) {
      bool result = false;
      if (myObject is Matrix) {
        var parameter = myObject as Matrix;
        if (parameter.RowAndLineCount == this.RowAndLineCount) {
          result = true;
        }
      }
      return result;
    }

    //Взять код элемента
    public override int GetHashCode() {
      return (int)this.RowAndLineCount;
    }

    //Неявное преобразования
    public static implicit operator double[,](Matrix inputMatrix) {
      return inputMatrix.InnerPartOfMatrix;
    }

    //Явное преобразования
    public static explicit operator double(Matrix inputMatrix) {
      return inputMatrix.RowAndLineCount;
    }

    //Оператор True
    public static bool operator true(Matrix inputMatrix) {
      return inputMatrix.RowAndLineCount != 1;
    }

    //Оператор False
    public static bool operator false(Matrix inputMatrix) {
      return inputMatrix.RowAndLineCount == 1;
    }

    //Оператор CompareTo
    int IComparable.CompareTo(object myObject) {
      if (myObject is Matrix) {
        var parameter = myObject as Matrix;
        if (parameter.RowAndLineCount > this.RowAndLineCount) {
          Console.WriteLine("1 case");
          return -1;
        }
        if (parameter.RowAndLineCount == this.RowAndLineCount) {
          Console.WriteLine("2 case");
          return 0;
        }
        if (parameter.RowAndLineCount < this.RowAndLineCount) {
          Console.WriteLine("3 case");
          return 1;
        }
      }
      return -1;
    }

  }

  //Класс расширяющихся методов
  public static class ExtensionsMetods {
    private static double traceOfMatrix = 0;
    private static Matrix resultMatrix = new Matrix();

    //След матрицы
    public static double MatrixTrace(this Matrix inputMatrix) {
      for (int rowIndex = 0; rowIndex < inputMatrix.RowAndLineCount; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < inputMatrix.RowAndLineCount; ++columnIndex) {
          if (inputMatrix.InnerPartOfMatrix[rowIndex, columnIndex] == inputMatrix.InnerPartOfMatrix[columnIndex, rowIndex]) {
            traceOfMatrix += inputMatrix.InnerPartOfMatrix[rowIndex, columnIndex];
          }
        }
      }
      return traceOfMatrix;
    }

    //Транспонирование
    public static Matrix MatrixTranspose(this Matrix inputMatrix) {
      for (int rowIndex = 0; rowIndex < inputMatrix.RowAndLineCount; ++rowIndex) {
        Console.Write("\n");
        for (int columnIndex = 0; columnIndex < inputMatrix.RowAndLineCount; ++columnIndex) {
          Console.Write(inputMatrix.InnerPartOfMatrix[columnIndex, rowIndex] + " ");
        }
      }
      return resultMatrix;
    }
  }

  //Предок всех событий
  public abstract class AncestorOfEvents {
    public string EventType { get; set; }
    public virtual void Call(Matrix inputMatrix) {

    }
  }

  //Событие сложения
  class SummEvent : AncestorOfEvents {
    public SummEvent() {
      EventType = "Summ";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((inputMatrix + inputMatrix).ToString() + "\n"); ;
    }
  }

  //Событие вычитания
  class DifferenceEvent : AncestorOfEvents {
    public DifferenceEvent() {
      EventType = "Difference";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((inputMatrix - inputMatrix).ToString() + "\n");
    }
  }

  //Событие сравнения
  class CompareEvent : AncestorOfEvents {
    public CompareEvent() {
      EventType = "Compare";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((inputMatrix == inputMatrix).ToString() + "\n");
    }
  }

  //Событие умножения
  class MultiplicationEvent : AncestorOfEvents {
    public MultiplicationEvent() {
      EventType = "Multiplication";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((inputMatrix * inputMatrix).ToString() + "\n");
    }
  }

  //Событие обратной матрицы
  class ReverseEvent : AncestorOfEvents {
    public ReverseEvent() {
      EventType = "Reverse";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((+inputMatrix).ToString() + "\n");
    }
  }

  //Событие определителя
  class DeterminantEvent : AncestorOfEvents {
    public DeterminantEvent() {
      EventType = "Determinant";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine((!inputMatrix).ToString() + "\n");
    }
  }

  //Событие следа
  class TraceEvent : AncestorOfEvents {
    public TraceEvent() {
      EventType = "Trace";
    }
    public override void Call(Matrix inputMatrix) {
      double trace = inputMatrix.MatrixTrace();
      Console.WriteLine(trace);
    }
  }

  //Событие транспонирования
  class TransposeEvent : AncestorOfEvents {
    public TransposeEvent() {
      EventType = "Transpose";
    }
    public override void Call(Matrix inputMatrix) {
      inputMatrix = inputMatrix.MatrixTranspose();
    }
  }

  //Событие диагональной матрицы
  class DiagonalEvent : AncestorOfEvents {
    public DiagonalEvent() {
      EventType = "Diagonal";
    }
    public override void Call(Matrix inputMatrix) {
      Console.WriteLine("Hello, I am anonymous method");
      MatrixToDiag anonymousMethod = delegate (Matrix myNewMatrix) {
        inputMatrix.MatrixToDiagonal(inputMatrix);
      };
      anonymousMethod.Invoke(inputMatrix);
    }
  }

  class LonelyEvent : AncestorOfEvents {
    public LonelyEvent() {
      EventType = "Lonely, no operation";
    }
  }

  // Предок всех обработчиков
  public abstract class AncestorOfHandler {
    protected AncestorOfEvents innerEvent { get; set; }
    protected AncestorOfHandler next { get; set; }

    public AncestorOfHandler() {
      next = null;
    }
    public virtual void Handle(AncestorOfEvents inputEvent, Matrix inputMatrix) {
      if (innerEvent.EventType == inputEvent.EventType) {
        inputEvent.Call(inputMatrix);
        Console.WriteLine("{0} exist and executed", innerEvent.EventType);
      } else {
        Console.WriteLine("Trying to find event type, sending to next....");
        if (next != null) {
          next.Handle(inputEvent, inputMatrix);
        } else {
          Console.WriteLine("Event does not exist");
        }
      }
    }

    protected void SetNextHandler(AncestorOfHandler newHanlder) {
      next = newHanlder;
    }
  }

  //Обработчики
  class Handler1 : AncestorOfHandler {
    public Handler1() {
      innerEvent = new SummEvent();
      next = new Handler2();
    }
  }

  class Handler2 : AncestorOfHandler {
    public Handler2() {
      innerEvent = new DifferenceEvent();
      next = new Handler3();
    }
  }

  class Handler3 : AncestorOfHandler {
    public Handler3() {
      innerEvent = new CompareEvent();
      next = new Handler4();
    }
  }
  class Handler4 : AncestorOfHandler {
    public Handler4() {
      innerEvent = new MultiplicationEvent();
      next = new Handler5();
    }
  }
  class Handler5 : AncestorOfHandler {
    public Handler5() {
      innerEvent = new ReverseEvent();
      next = new Handler6();
    }
  }

  class Handler6 : AncestorOfHandler {
    public Handler6() {
      innerEvent = new DeterminantEvent();
      next = new Handler7();
    }
  }
  class Handler7 : AncestorOfHandler {
    public Handler7() {
      innerEvent = new TraceEvent();
      next = new Handler8();
    }
  }

  class Handler8 : AncestorOfHandler {
    public Handler8() {
      innerEvent = new TransposeEvent();
      next = new Handler9();
    }
  }
  class Handler9 : AncestorOfHandler {
    public Handler9() {
      innerEvent = new DiagonalEvent();
      next = null;
    }
  }

  //Класс "Цепь подчинения" (честно украдено из лекции)
  public class ChainApplication {
    private AncestorOfHandler eventHandler;
    private int typeOfOperation;
    public ChainApplication(Matrix inputMatrix, int inputType) {
      eventHandler = new Handler1(); typeOfOperation = inputType;
    }
    public void Run(int eventCount, Matrix inputMatrix) {
      for (int eventIndex = 0; eventIndex < eventCount; ++eventIndex) {
        handleEvent(GenerateRandomEvent(inputMatrix), inputMatrix);
      }
    }
    public void handleEvent(AncestorOfEvents inputEvent, Matrix inputMatrix) {
      eventHandler.Handle(inputEvent, inputMatrix);
    }

    private AncestorOfEvents GenerateRandomEvent(Matrix inputMatrix) {
      AncestorOfEvents result;
      switch (typeOfOperation) {
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

  internal class Program {

    //Основная часть
    static void Main(string[] args) {
      Matrix myMatrix = new Matrix();
      int select = 0;
      ChainApplication eventSelector = new ChainApplication(myMatrix, select);
      Matrix myCloneMatrix = (Matrix)myMatrix.Clone(myMatrix.RowAndLineCount);
      Matrix resultMatrix = (Matrix)myMatrix.Clone(myMatrix.RowAndLineCount);
      bool IsRunning = true;
      char exit = ' ';

      //Показываем пользователю матрицы
      myMatrix.ShowMatrix(myMatrix);
      myCloneMatrix.ShowMatrix(myCloneMatrix);

      //Основной цикл работы основной части
      while (IsRunning) {
        Console.WriteLine("Select your operation: 0 = summ, 1 = difference, 2 = compare, 3 = multiplication, " +
                  "4 = reverse matrix, 5 = determinant, 6 = trace, 7 = transpOse, 8 = turn to diagonal");
        select = Convert.ToInt32(Console.ReadLine());

        eventSelector = new ChainApplication(myMatrix, select);
        eventSelector.Run(1, myMatrix);

        Console.WriteLine("Enough? y/n");
        exit = Convert.ToChar(Console.ReadLine());
        if (exit == 'y') {
          Console.WriteLine("Okay. Press any button to exit completely");
          IsRunning = false;
        } else {
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
