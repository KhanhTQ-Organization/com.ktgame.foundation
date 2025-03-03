namespace com.ktgame.foundation.patterns.behavioral.visitor
{
    /// <summary> Visitable interface. </summary>
    public interface IVisitable { }

    /// <summary> Visitable interface. </summary>
    public interface IVisitable<in T> where T : IVisitor
    {
        /// <summary> Accept a visitor's visit. </summary>
        void Accept(T visitor);
    }
}