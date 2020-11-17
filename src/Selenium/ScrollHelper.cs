namespace Musketeer.Selenium
{
    public static class ScrollHelper
    {
        public static void ScrollElementToBottom(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(0, arguments[0].scrollHeight);", element.Current);

        public static void ScrollElementToTop(Element element) =>
            Driver.ExecuteJs("arguments[0].scrollBy(0, -arguments[0].scrollHeight);", element.Current);

        public static void ScrollElementBy(Element element, int x, int y) =>
            Driver.ExecuteJs("arguments[0].scrollBy(x, y);", element.Current);

        public static void scrollElementByHeight(Element element) =>
            Driver.ExecuteJs($"arguments[0].scrollBy(0, {element.Current.Size.Height});", element.Current);
    }
}