namespace Task2.Tests;

using Task2;

public class Tests
{
    [Test]
    public void Circle_WithZeroRadius_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Circle(0));
    }

    [Test]
    public void Circle_WithNegativeRadius_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Circle(-1));
    }
    
    [Test]
    public void Circle_WithPositiveRadius_ReturnsCorrectSquare()
    {
        var circle = new Circle(5);
        
        var square = circle.GetSquare();
        
        Assert.That(square, Is.EqualTo(Math.PI * 25).Within(1e-10));
    }
    
    [Test]
    public void Triangle_WithPositiveSides_ReturnsCorrectSquare()
    {
        var triangle = new Triangle(3, 4, 5);
        
        var square = triangle.GetSquare();
        
        Assert.That(square, Is.EqualTo(6).Within(1e-10));
    }

    [Test]
    public void Triangle_WithOneSideZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Triangle(0, 4, 5));
    }

    [Test]
    public void Triangle_WithNegativeSide_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Triangle(3, -4, 5));
    }

    [Test]
    public void Triangle_WithSidesNotFormingTriangle_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Triangle(1, 2, 3));
    }

    [Test]
    public void Triangle_IsRectangular_ReturnsTrueForRightTriangle()
    {
        var triangle = new Triangle(3, 4, 5);
        
        var isRectangular = triangle.IsRectangular();
        
        Assert.That(isRectangular, Is.True);
    }

    [Test]
    public void Triangle_IsRectangular_ReturnsFalseForNonRightTriangle()
    {
        var triangle = new Triangle(3, 4, 6);
        
        var isRectangular = triangle.IsRectangular();

        Assert.That(isRectangular, Is.False);
    }
}