namespace LMSweb_v3.Extensions
{
    public static class ExtensionVectorOperation
    {
        /// <summary>
        /// 計算餘弦相似值，兩個向量的長度必須相同(兩向量已被正規化時使用)
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <returns>餘弦相似值</returns>
        public static float InnerProduct(this IEnumerable<float> vector1, IEnumerable<float> vector2)
        {
            return vector1.Zip(vector2, (a, b) => a * b).Sum();
        }

        /// <summary>
        /// 計算餘弦相似值，兩個向量的長度必須相同(兩向量未被正規化時使用)
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <returns>餘弦相似值</returns>
        public static float CosineSimilarity(this IEnumerable<float> vector1, IEnumerable<float> vector2)
        {
            var denominator = Math.Sqrt(vector1.InnerProduct(vector1)) * Math.Sqrt(vector2.InnerProduct(vector2));
            return vector1.InnerProduct(vector2) / (float)denominator;
        }
    }
}
