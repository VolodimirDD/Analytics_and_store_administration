SELECT REPLACE(FORMAT(SUM(ZR.[Кількість] * T.[Ціна]), 'N0'), ',', ' ') AS [Загальний прибуток, грн]
FROM Реалізація R
JOIN Зміст_реалізації ZR ON R.[ID_покупки] = ZR.[ID_покупки]
JOIN Товар T ON ZR.[ID_товару] = T.[ID_товару];