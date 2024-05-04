using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    public class TokenFactory : ITokenFactory
    {
        private readonly List<ITokenProvider> tokenProviders = new List<ITokenProvider>();
        private static readonly object _lock = new object();
        private static TokenFactory? _instance;

        public static TokenFactory Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock(_lock )
                    {
                        _instance ??= new TokenFactory();
                    }
                }
                return _instance;
            }
        }

        private TokenFactory()
        {
            this
                .RegisterProvider(new OperandTokenProvider())
                .RegisterProvider(new ParenthesisTokenProvider())
                .RegisterProvider(new OperatorTokenProvider());
        }

        public IToken? CreateToken(string token, params object[] tokenParams)
        {
            var tokenProvider = this.tokenProviders.FirstOrDefault(x => x.Predicate.Invoke(token));

            if (tokenProvider is null)
            {
                return null;
            }

            return tokenProvider.Provide(token, tokenParams);
        }

        private TokenFactory RegisterProvider(ITokenProvider tokenProvider)
        {
            this.tokenProviders.Add(tokenProvider);
            return this;
        }
    }
}
