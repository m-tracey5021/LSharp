using System;
using System.Collections.Generic;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Expression
    {
        Symbol root;
        public Expression(){}
        public void Sanitise(){ root.Sanitise(); }
        public void AppendNode(Symbol parent, Symbol child){
            parent.children.Add(child);
            Sanitise();
        }
        public void RemoveNode(Symbol node){
            List<Symbol> siblings = node.parent.children;
            siblings.Remove(node);
            Sanitise();
        }
        public void ReplaceNode(Symbol node, Symbol replacement){
            List<Symbol> siblings = node.parent.children;
            for (int i = 0; i < siblings.Count; i ++){
                if (siblings[i] == node){
                    siblings.Insert(i, replacement);
                    siblings.RemoveAt(i + 1);
                    Sanitise();
                    return;
                }
            }
        }
        public void InsertNode(Symbol node, Symbol replacement){

        }
    }
}