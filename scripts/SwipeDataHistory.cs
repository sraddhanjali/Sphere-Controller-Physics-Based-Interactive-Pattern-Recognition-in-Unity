using System;
using System.Collections.Generic;

class SwipeDataHistory<T>{
	Queue<T> data;

	public SwipeDataHistory(){
		data = new Queue<T>();
	}

	public void AddEntry(T newData){
		data.Enqueue(newData);
	}
		
	public void WriteToFile(){
		foreach (T d in data) {
			
		}
	}
}