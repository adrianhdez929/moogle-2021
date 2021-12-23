﻿using Utils;
namespace MoogleEngine {
    public static class Moogle {
        public static SearchResult Query(string query) {
            // Modifique este método para responder a la búsqueda

            FileHandler[] files = Utils.Utils.LoadFiles();

            SearchItem[] items = new SearchItem[3] {
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
            };

            return new SearchResult(items, query);
        }
    }
}
