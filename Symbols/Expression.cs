using System;
using System.Collections.Generic;
using MathSharp.Visitors;

namespace MathSharp.Symbols
{
    public class Expression
    {
        Symbol root;
        public Expression(){}
        public void sanitise(){ root.sanitise(); }
        public void appendNode(Symbol parent, Symbol child){
            parent.children.Add(child);
            sanitise();
        }
        public void removeNode(Symbol node){
            List<Symbol> siblings = node.parent.children;
            siblings.Remove(node);
            sanitise();
        }
        public void replaceNode(Symbol node, Symbol replacement){
            List<Symbol> siblings = node.parent.children;
            for (int i = 0; i < siblings.Count; i ++){
                if (siblings[i] == node){
                    siblings.Insert(i, replacement);
                    siblings.RemoveAt(i + 1);
                    sanitise();
                    return;
                }
            }
        }
        public void insertNode(Symbol node, Symbol replacement){

        }
    }
}