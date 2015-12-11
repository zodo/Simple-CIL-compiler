namespace Language.Semantic
{
    public static class OptimizeMode
    {
        public static bool UnreacheableCode { get; set; } = true;

        public static bool ExpressionSimplify { get; set; } = true;

        public static bool Variables { get; set; } = true;

        public static bool LoopExpansion { get; set; } = true;

        public static int LoopExpansionRepeatLimit { get; } = 3;
    }
}
