# PROPAS Tool Summary

## Short Summary

PROPAS (The **PRO**perty **PA**tten **S**pecification and Analysis) is a tool set for automated and formal consistency analysis of industrial critical requirements. It consists of three separate engines that provide the overall functionality:


### PROPAS UI 

  Is a GUI part of the PROPASS tool that provides the necessary means for generating formal system specifications based on the [Specification Patterns](http://patterns.projects.cs.ksu.edu/).


### SMTLibReq

  Is a library that takes as an input a set of TCTL properties and transforms them into an SMT-LIB (The Satisbiability Modulo Theories Library) script which can be used for consistency analysis.
  
   The transformation of the TCTL properties into SMT-LIB assertions is performed in following steps:
    
   * Parsing the TCTL formulas (given as strings) into a Binary Tree (BT) structure that isolates the syntax parts of the formula. The inner nodes of the Binary Tree represent the operators (path quantifiers, path specific temporal operators, logical operators, arithmetic operators, etc.), whereas the leaf nodes contain the atomic propositions from the formula.
    
  * Transformation of the (BT) structures into SMT-LIB assetions. For each of the formulas represented as BT structure the SMTLiBReq library creates two types of assertions:
    * declaration for the atomic propositions (be they SMT-LIB constants or functions).
    * constaints over the allowed set of values of that particular atomic proposition.
