namespace Incapsulation.RationalNumbers;

public class Rational
{
    public int Numerator { get; private set; }
    public int Denominator { get;  private set; }
    public bool IsNan => Denominator == 0;

    public Rational(int numerator, int denominator)
    {
        for (int i = Math.Abs(numerator); i >= 2; i--)
        {
            if (numerator % i == 0 && denominator % i == 0)
            {
                numerator /= i;
                denominator /= i;
            }
        }

        if ((numerator < 0 && denominator < 0) || denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
        
        Numerator = numerator;
        Denominator = denominator;
        
        if (numerator == 0 && denominator != 0)
            Denominator = 1;
    }
    
    public Rational(int numerator)
    {
        Numerator = numerator;
        Denominator = 1;
    }

    public static Rational operator +(Rational r1, Rational r2)
    {
        var numerator = (r1.Numerator * r2.Denominator) + (r2.Numerator *  r1.Denominator);
        var denominator = r1.Denominator * r2.Denominator;
        return new Rational(numerator, denominator);
    }
    
    public static Rational operator -(Rational r1, Rational r2)
    {
        r2.Numerator = -r2.Numerator;
        return r1 + r2;
    }
    
    public static Rational operator *(Rational r1, Rational r2)
    {
        var numerator = r1.Numerator * r2.Numerator;
        var denominator = r1.Denominator * r2.Denominator;
        return new Rational(numerator, denominator);
    }
    
    public static Rational operator /(Rational r1, Rational r2)
    {
        if (r2.IsNan)
            return new Rational(0, 0);
        
        (r2.Numerator, r2.Denominator) = (r2.Denominator, r2.Numerator);
        return r1 * r2;
    }
    
    public static implicit operator double(Rational r) 
        => r.IsNan ? double.NaN
            : (double)r.Numerator / r.Denominator;
    
    public static implicit operator Rational(int num) => new Rational(num);

    public static implicit operator int(Rational r) 
        => r.Numerator % r.Denominator == 0 
            ? r.Numerator / r.Denominator
            : throw new Exception();
}