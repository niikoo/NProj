#!/usr/bin/perl      
# The first line of the script envokes Perl 
# Use "/usr/bin/perl -w" option for debugging

# Scalar variables
$var1 = "Hello World";   
$var2 = 14.6;

# Array variables
@arr1 = (0,1,2,3,4);
@arr2 = ("zero","one","two","three","four");

# Hash variable, or associative array
%hash1 = ("one","Monday","two", "Tuesday","three", "Wednesday","four","Thursday");

# Some simple printing
print $var1; 

# Subroutine
sub test() {
	print "ok";
}