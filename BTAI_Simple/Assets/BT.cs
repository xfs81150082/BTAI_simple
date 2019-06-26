using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTAI
{
    public enum BTState
    {
        Failure,
        Success,
        Continue,
        Abort
    }

    public static class BT 
    {
        public static Root Root() { return new Root(); }
        public static Sequence Sequence() { return new Sequence(); }
        public static Selector Selector(bool shuffle = false) { return new Selector(shuffle); }

        public static Action Call(System.Action fn) { return new Action(fn); }
        public static Condition Condition(System.Func<bool> fn) { return new Condition(fn); }

        public static Trigger Trigger(Animator animator, string name, bool set = true) { return new Trigger(animator, name, set); }

    }

    public abstract class BTNode
    {
        public abstract BTState Tick();
    }

    public abstract class Branch : BTNode
    {
        protected int activeChild;
        protected List<BTNode> children = new List<BTNode>();
        public virtual Branch OpenBranch(params BTNode[] children)
        {
            for (var i = 0; i < children.Length; i++)
                this.children.Add(children[i]);
            return this;
        }

        public List<BTNode> Children()
        {
            return children;
        }

        public int ActiveChild()
        {
            return activeChild;
        }

        public virtual void ResetChildren()
        {
            activeChild = 0;
            for (var i = 0; i < children.Count; i++)
            {
                Branch b = children[i] as Branch;
                if (b != null)
                {
                    b.ResetChildren();
                }
            }
        }
    }

    public class Sequence : Branch
    {
        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    else
                        return BTState.Continue;
                case BTState.Failure:
                    activeChild = 0;
                    return BTState.Failure;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("This should never happen, but clearly it has.");
        }
    }

    public class Selector : Branch
    {
        public Selector(bool shuffle)
        {
            if (shuffle)
            {
                var n = children.Count;
                while (n > 1)
                {
                    n--;
                    var k = Mathf.FloorToInt(Random.value * (n + 1));
                    var value = children[k];
                    children[k] = children[n];
                    children[n] = value;
                }
            }
        }

        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild = 0;
                    return BTState.Success;
                case BTState.Failure:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Failure;
                    }
                    else
                        return BTState.Continue;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("This should never happen, but clearly it has.");
        }
    }

    public abstract class Block : Branch
    {
        public override BTState Tick()
        {
            switch (children[activeChild].Tick())
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    return BTState.Continue;
            }
        }
    }

    //Node
    public class Root : Block
    {
        public bool isTerminated = false;

        public override BTState Tick()
        {
            if (isTerminated) return BTState.Abort;
            while (true)
            {
                switch (children[activeChild].Tick())
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    case BTState.Abort:
                        isTerminated = true;
                        return BTState.Abort;
                    default:
                        activeChild++;
                        if (activeChild == children.Count)
                        {
                            activeChild = 0;
                            return BTState.Success;
                        }
                        continue;
                }
            }
        }
    }

    public class Action : BTNode
    {
        System.Action fn;
        System.Func<IEnumerator<BTState>> coroutineFactory;
        IEnumerator<BTState> coroutine;
        public Action(System.Action fn)
        {
            this.fn = fn;
        }
        public Action(System.Func<IEnumerator<BTState>> coroutineFactory)
        {
            this.coroutineFactory = coroutineFactory;
        }
        public override BTState Tick()
        {
            if (fn != null)
            {
                fn();
                return BTState.Success;
            }
            else
            {
                if (coroutine == null)
                    coroutine = coroutineFactory();
                if (!coroutine.MoveNext())
                {
                    coroutine = null;
                    return BTState.Success;
                }
                var result = coroutine.Current;
                if (result == BTState.Continue)
                    return BTState.Continue;
                else
                {
                    coroutine = null;
                    return result;
                }
            }
        }

        public override string ToString()
        {
            return "Action : " + fn.Method.ToString();
        }
    }

    public class Condition : BTNode
    {
        public System.Func<bool> fn;

        public Condition(System.Func<bool> fn)
        {
            this.fn = fn;
        }
        public override BTState Tick()
        {
            return fn() ? BTState.Success : BTState.Failure;
        }

        public override string ToString()
        {
            return "Condition : " + fn.Method.ToString();
        }
    }

    public class Trigger : BTNode
    {
        Animator animator;
        int id;
        string triggerName;
        bool set = true;

        //if set == false, it reset the trigger istead of setting it.
        public Trigger(Animator animator, string name, bool set = true)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.triggerName = name;
            this.set = set;
        }

        public override BTState Tick()
        {
            if (set)
                animator.SetTrigger(id);
            else
                animator.ResetTrigger(id);

            return BTState.Success;
        }

        public override string ToString()
        {
            return "Trigger : " + triggerName;
        }
    }
}
