using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1
{
    public class CustomStack<T>
    {
        LinkedList<T> stack = new LinkedList<T>();

        public void Push(T t)
        {
            if (stack.Count >= Capacity)
            {
                stack.RemoveFirst();
            }
            stack.AddLast(t);
        }

        public T Pop()
        {
            if (stack.Count == 0)
            {
                throw new Exception("The stack is empty.");
            }
            var result = stack.Last.Value;
            stack.RemoveLast();
            return result;
        }

        public T Peek(T t)
        {
            if (stack.Count == 0)
            {
                throw new Exception("The stack is empty.");
            }
            return stack.Last.Value;
        }

        public void RemoveFirst()
        {
            stack.RemoveFirst();
        }

        public void Clear()
        {
            stack.Clear();
        }

        public int Count { get { return stack.Count; } }

        public int Capacity { get; set; }
    }
}
