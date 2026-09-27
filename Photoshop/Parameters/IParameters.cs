namespace MyPhotoshop.Parameters;

public interface IParameters
{
    /// <summary>
    /// Метод, возвращающий информацию о настройках
    /// </summary>
    /// <returns>возвращает массив ParameterInfo[] - информация о настройках</returns>
    ParameterInfo[] GetDescription();
    
    /// <summary>
    /// Метод, устанавливающий поля класса в соответствие с массивом переданных величин
    /// </summary>
    /// <param name="parameters"></param>
    void Parse(double[] parameters);
}