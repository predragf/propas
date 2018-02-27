# PROPAS Tool Summary

## Short Summary

PROPAS (The **PRO**perty **PA**tten **S**pecification and Analysis) is a tool set for automated and formal consistency analysis of industrial critical requirements based on Satisfiability Modulo Theories, which is available [here](http://www.es.mdh.se/publications/4583-SMT_based_Consistency_Analysis_of_Industrial_Systems_Requirements).


It consists of three separate engines that provide the overall functionality:


### PROPAS UI 

  Is a GUI part of the PROPASS tool that provides the necessary means for generating formal system specifications based on the [Specification Patterns](http://patterns.projects.cs.ksu.edu/).


### SMTLibReq

  Is a library that takes as an input a set of TCTL properties and transforms them into an SMT-LIB (The Satisbiability Modulo Theories Library) script which can be used for consistency analysis.
  
   The transformation of the TCTL properties into SMT-LIB assertions is performed in following steps:
    
   * Parsing the TCTL formulas (given as strings) into a Binary Tree (BT) structure that isolates the syntax parts of the formula. The inner nodes of the Binary Tree represent the operators (path quantifiers, path specific temporal operators, logical operators, arithmetic operators, etc.), whereas the leaf nodes contain the atomic propositions from the formula.
    
  * Transformation of the (BT) structures into SMT-LIB assetions. For each of the formulas represented as BT structure the SMTLiBReq library creates two types of assertions:
    * declaration for the atomic propositions (be they SMT-LIB constants or functions).
    * constaints over the allowed set of values of that particular atomic proposition.
    
    
### SMTLibReq ###

#### What is SMTLibReq? ####

SMTLibReq is the part of the PROPAS tool which generates an SMT-LIB script suitable for checking based
on the formal system specification encoded in TCTL. The parser operates according to the 1:1 principle, meaning that there is a one to one mapping between the TCTL properties and the SMT-Lib assertions that constiture the SMT-Lib script. The library supports parsing and transformation of arbitrary nested TCTL formulas.

The complete transformation of TCTL properties into SMT-LIB assertions is divided into two major steps:

* parsing the TCTL formulas into an intermediate format (structure) suitable for transformation
* transforming the intermediate format into an SMT-Lib assertion

In the following, we briefly sketch the high level overview of the above steps.

##### Parsing the TCTL formulas #####

The system requirements specification is provided as a list of strings where each string encodes a system requirement. In order to be able to transform the TCTL properties into corresponding SMT-Lib assertions, first we have to parse the TCTL strings into a structure that recognizes and encodes all the syntactical elements of a TCTL property such that the appropriate semantics can be attached to all of them.

First, let us briefly dissect a TCTL property. A TCTL property represents a specific encoding of a temporal logic property. The interpretation of a TCTL property is over a Kripke structure. Therefore, the syntax of TCTL consists of path quantifiers All (denoted as A) and Exists (denoted as E), and path-specific temporal operators G (Globally, or for all states) and F (Future/Eventually). The universal path quantifier “A” stands for “all paths”, while the existential quantifier “E” denotes that “there exists a path” from the set of all future paths starting from a given state s.    

In order to be able to automatically reason about this formula, first we have to identify all the syntactic elements. To be able to detect which structure is the most apropriate for encoding the syntactic elements, first we divide them into two major categories: i) atomic propositions and ii) operators. The definition for atomic propositions is as usuall one, whereas the operators represent a union of the valid TCTL path-specific temporal (G, F, U, W), TCTL branch quantifiers (A, E) and the standard logical operators (and, or, =>, negation). 

It is evident, that all of the used operators are either unary or binary. Driven by this fact we organize the syntactic elements of the formula into a binary-tree structure, organized from the least- to the most binding operators. The parsing process is performed in the following way: first, we identify the temporal and the path quantifiers of the formula and then parse the propositional formula to the atomic constituents. Then we parse the propositional expressoion, where we first extract the operators and then the atomic propositions. To better illustrate the procedure, lets let us look at the formula f: 

AG(p => q), which reads "It is always the case that p implies q" and means "for all possible paths, and for all possible states p implies q".

In this particular example, we have one temporal (A) and branching operator (G). The proposition of the fomula consists of two atomic propositions (p and q) and an implication (denoted as =>). Now that we have identified all the syntactic parts of the formula, and based on the binding order of the same, we construct the following binary tree:

				     A
				     |
				     G
				     |
				     =>
				_____||_____
				|	    |
				p	    q

In the resulting binary tree, all the leaf nodes correspond to the atomic propositions, whereas the rest of the nodes are representing the operators of the formula. The binary tree constructed in this way, can be parsed in a efficient way in either top-down or bottom up approach. 

The way the TCTL formula binary trees are transformed into SMT-Lib assertion is described below.


##### Parsing the TCTL formulas #####


The SMTLibReq library provides a template-based transformation of TCTL formulas into SMT-Lib assertions. As presented in the previous section, the input to the transformation procedure is the binary-tree reprentation of the formula. 

The implemented transformation algorithm works as follows:
	The traversal of the binary-tree representation of the TCTL formula is done in a top-down manner. Each of the operators in the formula are transformed using a predefined template. In order to be able to specify timing requirements, each atomic proposition is a function of time, which in the provided tranformation (encoding) is represented as real-valued variable. For the details on the proposed transformation approach we reffer our readers to [1].
	
For illustration, let us revisit the previous example and show how the TCTL formula is transformed into an SMT-Lib assertion.

The procedure starts from the root node. Since the current implementation supports only universaly quantified path operators, they are automatically merged into the path-specific temporal operators. This being said, the transformation beings by the transformation engine recognizing the AG combination and finding an appropriate template for it. The template for that combination is (forall ((time Real)) (expression time)). Then the procedure continues with the next node which in this case is the implication (=>). Again, an adequate template is detemined by the engine and expression is now replaced with (=> (left time) (right time)), which yileds the overall formula to become: (forall ((time Real)) (=> (left time) (right time))). The final step of the transformation is to substite the "left" and "right" with the atomic propositions from the original formula. After the last step, the final form of the SMT-Lib assertion corresponding to the above TCTL formula looks like follows: (forall ((time Real)) (=> (p time) (q time))).

#### The Required Formatting of the input TCTL formulas ####

Due to the support of the arbitrary nesting of TCTL formulas and in order to be able to preserve the correct evaluation order, the scope of the temporal operators (that is the extent of the formula to which the given operators apply) must be explicitly denoted with parentesis (). So, for example the formula
AG p => AFq must be specified as AG(p => AF(q)).

SMTLibReq supports both timed and untimed version of the path-specific temporal operators. The timed version of the path-specific operator is specified as: Oper~T, where Oper e \[G, F, U, W], ~ e \[<, <=] and T is a numerical constant.

#### Limitations ####

The implementation of the SMTLibReq is within the frames of the proposed theoretical framework for the automated analysis of safety-critical requirements using SMT solvers. This means that in the current version, we are able to transform (and check for consistency) only requirements which are universaly quantified over branches. 


### Z3 SMT Solver

  The current version of the PROPAS tool uses Z3 from Microsoft Research as consistency checking engine.

### Who do I talk to? ###

For any inquiry please contact the developer: Predrag Filipovikj <predrag.filipovikj@mdh.se>.
