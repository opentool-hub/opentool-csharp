using System;
using System.Collections.Generic;

namespace OpenToolSDK.Example
{
    public class MockUtil
    {
        private readonly List<string> _storage = new List<string> { "Hello", "World" };

        public int Count()
        {
            return _storage.Count;
        }

        public int Create(string text)
        {
            _storage.Add(text);
            return _storage.Count - 1;
        }

        public string Read(int id)
        {
            if (id < 0 || id >= _storage.Count)
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid ID.");

            return _storage[id];
        }

        public void Update(int id, string text)
        {
            if (id < 0 || id >= _storage.Count)
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid ID.");

            _storage[id] = text;
        }

        public void Delete(int id)
        {
            if (id < 0 || id >= _storage.Count)
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid ID.");

            _storage.RemoveAt(id);
        }

        public void Run()
        {
            throw new Exception("A fatal error to break this tool.");
        }
    }
}