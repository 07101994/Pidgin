using System;
using System.Collections.Immutable;

namespace Pidgin
{
    public static partial class Parser<TToken>
    {
        /// <summary>
        /// Creates a parser which always fails without consuming any input.
        /// </summary>
        /// <param name="message">A custom error message</param>
        /// <typeparam name="T">The return type of the resulting parser</typeparam>
        /// <returns>A parser which always fails</returns>
        public static Parser<TToken, T> Fail<T>(string message = "Failed")
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return new FailParser<T>(message);
        }
        
        private sealed class FailParser<T> : Parser<TToken, T>
        {
            private readonly string _message;

            public FailParser(string message)
            {
                _message = message;
            }

            private protected override ImmutableSortedSet<Expected<TToken>> CalculateExpected()
                => ExpectedUtil<TToken>.Nil;

            internal sealed override InternalResult<T> Parse(ref ParseState<TToken> state)
            {
                state.Error = new ParseError<TToken>(
                    Maybe.Nothing<TToken>(),
                    false,
                    Expected,
                    state.SourcePos,
                    _message
                );
                return InternalResult.Failure<T>(false);
            }
        }
    }
}