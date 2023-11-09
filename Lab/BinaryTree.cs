namespace Console;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
{
    public BinaryTree()
    {
        root = null;
    }

    public void Insert(T data)
    {
        if (root == null)
        {
            root = new Node<T>(data);
            return;
        }
        InsertRecursive(root, data);
    }

    private void InsertRecursive(Node<T> node, T data)
    {
        int result = data.CompareTo(node.Data);
        //Smaller or equal values go to the left to prevent issues
        if (result <= 0)
        {
            if (node.Left == null)
            {
                node.Left = new Node<T>(data);
                return;
            }
            InsertRecursive(node.Left, data);
        }
        else
        {
            if (node.Right == null)
            {
                node.Right = new Node<T>(data);
                return;
            }
            InsertRecursive(node.Right, data);
        }
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return new InternalEnumerator<T>(root);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private Node<T>? root;

    //Preorder traversal
    private class InternalEnumerator<T> : IEnumerator<T> where T : IComparable<T>
    {
        private Node<T>? root;
        private Node<T>? current;
        private Stack<Node<T>> stack;
        
        public T Current => current.Data;
        object System.Collections.IEnumerator.Current => Current;
        
        public InternalEnumerator(Node<T>? root)
        {
            this.root = root;
            current = null;
            stack = new Stack<Node<T>>();
        }
        
        public bool MoveNext()
        {
            if (current == null)
            {
                current = root;
                return true;
            }
            if (current.Left != null)
            {
                stack.Push(current);
                current = current.Left;
                return true;
            }
            if (current.Right != null)
            {
                current = current.Right;
                return true;
            }
            if (stack.Count > 0)
            {
                Node<T> temp = stack.Pop();
                if (temp.Right != null)
                {
                    current = temp.Right;
                    return true;
                }
                return MoveNext();
            }
            return false;
        }
        
        public void Reset()
        {
            current = null;
            stack.Clear();
        }

        public void Dispose()
        {
            root = null;
            current = null;
            stack.Clear();
        }
    }
    
    private class Node<T> : IComparable<Node<T>> where T : IComparable<T>
    {
        public T Data { get; set; }
        public Node<T>? Left { get; set; }
        public Node<T>? Right { get; set; }
        
        public Node(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
        
        public int CompareTo(Node<T>? other)
        {
            return other == null ? 1 : Data.CompareTo(other.Data);
        }
    }
}