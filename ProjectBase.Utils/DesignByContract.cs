using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjectBase.Utils
{
    /// <summary>
    /// 契约式设计检查类.
    /// 如果不满足要求，UseAssertions为true时断言，否生成一个导常
    /// </summary>
    /// <remarks>
    /// This example shows how to call the Require method.
    /// <code>
    /// public void Test(int x)
    /// {
    /// 	try
    /// 	{
    ///			Check.Require(x > 1, "x must be > 1");
    ///		}
    ///		catch (System.Exception ex)
    ///		{
    ///			Console.WriteLine(ex.ToString());
    ///		}
    ///	}
    /// </code>
    ///
    /// You can direct output to a Trace listener. For example, you could insert
    /// <code>
    /// Trace.Listeners.Clear();
    /// Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
    /// </code>
    /// 
    /// or direct output to a file or the Event Log.
    /// 
    /// (Note: For ASP.NET clients use the Listeners collection
    /// of the Debug, not the Trace, object and, for a Release build, only exception-handling
    /// is possible.)
    /// </remarks>
    /// 
    public sealed class Check
    {
        #region Interface

        /// <summary>
        /// 前期条件检查 - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PreconditionException(message);
            }
            else
            {
                Trace.Assert(!assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PreconditionException(message, inner);
            }
            else
            {
                Trace.Assert(!assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PreconditionException("Precondition failed.");
            }
            else
            {
                Trace.Assert(!assertion, "Precondition failed.");
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PostconditionException(message);
            }
            else
            {
                Trace.Assert(!assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PostconditionException(message, inner);
            }
            else
            {
                Trace.Assert(!assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion)
        {
            if (UseExceptions)
            {
                if (assertion) throw new PostconditionException("Postcondition failed.");
            }
            else
            {
                Trace.Assert(!assertion, "Postcondition failed.");
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (assertion) throw new InvariantException(message);
            }
            else
            {
                Trace.Assert(!assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (assertion) throw new InvariantException(message, inner);
            }
            else
            {
                Trace.Assert(!assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion)
        {
            if (UseExceptions)
            {
                if (assertion) throw new InvariantException("Invariant failed.");
            }
            else
            {
                Trace.Assert(!assertion, "Invariant failed.");
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (assertion) throw new AssertionException(message);
            }
            else
            {
                Trace.Assert(!assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (assertion) throw new AssertionException(message, inner);
            }
            else
            {
                Trace.Assert(!assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion)
        {
            if (UseExceptions)
            {
                if (assertion) throw new AssertionException("Assertion failed.");
            }
            else
            {
                Trace.Assert(!assertion, "Assertion failed.");
            }
        }

        /// <summary>
        /// Set this if you wish to use Trace Assert statements 
        /// instead of exception handling. 
        /// (The Check class uses exception handling by default.)
        /// </summary>
        public static bool UseAssertions
        {
            get
            {
                return useAssertions;
            }
            set
            {
                useAssertions = value;
            }
        }

        #endregion // Interface

        #region Implementation

        // No creation
        private Check() { }

        /// <summary>
        /// Is exception handling being used?
        /// </summary>
        private static bool UseExceptions
        {
            get
            {
                return !useAssertions;
            }
        }

        // Are trace assertion statements being used? 
        // Default is to use exception handling.
        private static bool useAssertions = false;

        #endregion // Implementation

    } // End Check

    #region Exceptions

    /// <summary>
    /// Exception raised when a contract is broken.
    /// Catch this exception type if you wish to differentiate between 
    /// any DesignByContract exception and other runtime exceptions.
    ///  
    /// </summary>
    [Serializable]
    public class DesignByContractException : Exception
    {
        public DesignByContractException() { }
        public DesignByContractException(string message) : base(message) { }
        public DesignByContractException(string message, Exception inner) : base(message, inner) { }
        protected DesignByContractException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception raised when a precondition fails.
    /// </summary>
    [Serializable]
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner) : base(message, inner) { }
        protected PreconditionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    [Serializable]
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException() { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message) : base(message) { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner) : base(message, inner) { }

        protected PostconditionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception raised when an invariant fails.
    /// </summary>
    [Serializable]
    public class InvariantException : DesignByContractException
    {
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException() { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message) : base(message) { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message, Exception inner) : base(message, inner) { }
        protected InvariantException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception raised when an assertion fails.
    /// </summary>
    [Serializable]
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException() { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message) : base(message) { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner) : base(message, inner) { }
        protected AssertionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    #endregion // Exception classes
}
