namespace Musketeer.Selenium
{
    public interface IStorage
    {
        string GetItem(string key);

        void SetItem(string key, string value);

        void RemoveItem(string key);

        string Key(int index);

        int Length();

        void Clear();
    }
}
