namespace Task2;

/// <summary>
/// Абстрактный класс, представляющий геометрическую фигуру.
/// </summary>
public abstract class Figure
{
  /// <summary>
  /// Вычисляет площадь фигуры.
  /// </summary>
  /// <returns>Площадь фигуры как вещественное число.</returns>
  public abstract double GetSquare();
}

public class Circle : Figure
{
  public double Radius { get; private set; }

  /// <summary>
  /// Инициализирует новый экземпляр класса Circle с заданным радиусом.
  /// </summary>
  /// <param name="radius">Радиус круга. Обязательно положительное число.</param>
  /// <exception cref="ArgumentException">Бросается, если радиус не является строго положительным числом.</exception>
  public Circle(double radius)
  {
    if (radius <= 0) throw new ArgumentException("The radius must be positive numbers");
    Radius = radius;
  }
  
  public override double GetSquare() => Math.PI * Math.Pow(Radius, 2);
}

public class Triangle : Figure
{
  public double A { get; private set; }
  public double B { get; private set; }
  public double C { get; private set; }

  /// <summary>
  /// Инициализирует новый экземпляр класса Triangle с заданными сторонами.
  /// </summary>
  /// <exception cref="ArgumentException">Бросается, если стороны не являются строго положительными числами или
  /// треугольник с такими сторонами не существует.</exception>
  public Triangle(double a, double b, double c)
  {
    if (a <= 0 || b <= 0 || c <= 0)
      throw new ArgumentException("The sides must be positive numbers");
    
    if (!IsMayExist(a, b, c))
      throw new ArgumentException("A triangle with such sides does not exist. " +
                                  "The sum of any two sides must be greater than the third.");

    (A, B, C) = (a, b, c);
  }
  
  public override double GetSquare()
  {
    var p = (A + B + C) / 2;
    return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
  }

  /// <summary>
  /// Определяет, является ли треугольник прямоугольным.
  /// </summary>
  /// <returns>Истина, если треугольник прямоугольный, иначе ложь.</returns>
  public bool IsRectangular()
  {
    const double tolerance = 1e-10;
    return Math.Abs(A * A - (B * B + C * C)) < tolerance ||
           Math.Abs(B * B - (A * A + C * C)) < tolerance ||
           Math.Abs(C * C - (A * A + B * B)) < tolerance;
  }
  
  private static bool IsMayExist(double a, double b, double c) => a + b > c && b + c > a && c + a > b;
}