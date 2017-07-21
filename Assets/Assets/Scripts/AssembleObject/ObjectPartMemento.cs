using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMemento {
	private Vector3 position;
	public Vector3 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}
	public PartMemento(Vector3 position) {
		this.position = position;
	}
}

public class PartHistory {

	private Stack<PartMemento> history = new Stack<PartMemento>();

	public void SaveState(PartMemento memento) {
		history.Push(memento);
	}

	public Vector3 GetState() {
		return history.Pop().Position;
	}

	public bool IsEmpty() {
		if (history.Count == 0)
			return true;
		else
			return false;
	}
}