using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SelectionMethod {

	public SelectionMethod() {

	}

	//override on each specific selection class
	public abstract List<Individual> selectIndividuals (List<Individual> oldpop, int num); 

}
