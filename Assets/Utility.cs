using System.Collections;
using System.Collections.Generic;

public static class Utility {

    // Fisher-Yates shuffle https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    public static T[] ShuffleArray<T>(T[] array, int seed) {
        System.Random prng = new System.Random(seed);

        // skip last iteration since it is useless with the Fisher-Yates shuffle method
        for (int i = 0; i < array.Length - 1; i++) {
            int randomIndex = prng.Next(i, array.Length);
            T tempitem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempitem;
        }

        return array;
    }

}
