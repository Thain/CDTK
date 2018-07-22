using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{
    // Class to represent 
    public class LineList
    {
        public LineListNode zero;
        int Length = 0;

        // Adds an edge into the list, making sure it is in the correct place in the list so it is always sorted.
        public void AddSorted(Line2D edge, float edgeAngle){
            Length++;
            if(zero == null)
                zero = new LineListNode(edge, edgeAngle);
            else{
                LineListNode cur = zero.next;
                while(edgeAngle - cur.angle > .000001 && cur != zero)
                    cur = cur.next;
                LineListNode newLLN = new LineListNode(edge, cur, cur.prev, edgeAngle);
                cur.prev.next = newLLN;
                cur.prev = newLLN;
            }
        }
        // Removes a  vertex
        public void RemoveSorted(LineListNode l){
            l.prev.next = l.next;
            l.next.prev = l.prev;
            l = null;
        }
        public int GetLength(){return Length;}

        public LineListNode Get(Line2D line){
            LineListNode cur = zero;
            while(cur.value != line) cur = cur.next;
            return cur;
        }

    }

    public class LineListNode
    {
        protected internal Line2D value;
        protected internal float angle;
        protected internal LineListNode next;
        protected internal LineListNode prev;

        public LineListNode(Line2D v, float a){value = v; next = this; prev = this; angle = a;}
        public LineListNode(Line2D v, LineListNode n, LineListNode p, float a){value = v; next = n; prev = p; angle = a;}

        public float GetAngle(){return angle;}
        // public void SetValue(Line2D v){value = v;}
        // public Line2D GetValue(){return value;}
        // public void SetNext(LineLinkedListNode n){next = n;}
        // public LineLinkedListNode GetNext(){return next;}
        // public void SetPrev(LineLinkedListNode p){prev = p;}
        // public LineLinkedListNode GetPrev(){return prev;}
    }
}