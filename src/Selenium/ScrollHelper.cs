namespace Musketeer.Selenium
{
    public static class ScrollHelper
    {
        public static void ScrollElementToBottom(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(0, arguments[0].scrollHeight);", element.Current);

        public static void ScrollElementToTop(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(0, -arguments[0].scrollHeight);", element.Current);
        
        public static void ScrollElementToRight(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(arguments[0].scrollWidth, 0);", element.Current);

        public static void ScrollElementToLeft(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(-arguments[0].scrollWidth, 0);", element.Current);

        public static void ScrollElementBy(Element element, int x, int y) =>
            Driver.ExecuteJs($"arguments[0].scrollBy({x}, {y});", element.Current);

        public static void ScrollElementByHeight(Element element) =>
            Driver.ExecuteJs($"arguments[0].scrollBy(0, {element.Current.Size.Height});", element.Current);
        
        public static void ScrollElementByWidth(Element element) =>
            Driver.ExecuteJs($"arguments[0].scrollBy({element.Current.Size.Width}, 0);", element.Current);
        
        public static void ScrollElementIntoView(Element element) =>
            Driver.ExecuteJs($"arguments[0].scrollIntoView();", element.Current);
            
    }
}