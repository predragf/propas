# PROPAS (The **PRO**perty **PA**tten **S**pecification and Analysis)

## Short Summary

PROPAS is a tool set for automated and formal consistency analysis of industrial critical requirements. It consists of three separate engines that provide the overall functionality:


### PROPAS UI 

  Is a GUI part of the PROPASS tool that provides the necessary means for generating formal system specifications based on the [Specification Patterns](http://patterns.projects.cs.ksu.edu/).


### SMTLibReq

  Is a library that takes as an input a set of TCTL properties and transforms them into an SMT-LIB (The Satisbiability Modulo Theories Library) script which can be used for consistency analysis.
  
   The transformation of the TCTL properties into SMT-LIB assertions is performed in following steps:
