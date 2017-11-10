#!/usr/bin/python2
from random import random

compare_list = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]

def fixed_list_position():
  num = 0
  num_dict = {}
  for i in xrange(0, 3):
    for j in xrange(0, 3):
      num += 1
      num_dict[num] = [i, j]
  return num_dict  


def get_neighbours_of_a_number(num, num_dict):
  loc_num = num_dict[num]
  neighbours = []
  max_length_to_go = 3
  # increase either
  x = loc_num[0]
  y = loc_num[1]
  if x+1 < max_length_to_go:
    if y+1 < max_length_to_go:
      neighbours.append([x, y+1])
      neighbours.append([x+1, y])
      neighbours.append([x+1, y+1])
    if y-1 >= 0:
      neighbours.append([x+1, y-1])
  
  if x-1 >= 0:
    if y-1 >= 0:
      neighbours.append([x, y-1])
      neighbours.append([x-1, y])
      neighbours.append([x-1, y-1])
    if y+1 < 3:
      neighbours.append([x-1, y+1])
  
  nei_numbers = []
  for loc in neighbours:
    nei_numbers.append(compare_list[loc[0]][loc[1]])
    
  return nei_numbers
    

def valid_pattern_or_not_checker(num_dict, list_to_check):
  for i, item in enumerate(list_to_check):
    if i <= len(list_to_check) - 2:
      next_item = list_to_check[i+1]
      nei_numbers = get_neighbours_of_a_number(item, num_dict)
      if next_item not in nei_numbers:
        return False
  return True
    

def get_patterns_in_2D_list():
  num_dict = fixed_list_position()
  valid_patterns = []
  with open("patterns.txt", "r") as f:
    for line in f:
      temp = []
      for c in line:
        #print "c: " + c
        try:
          temp.append(int(c))
        except ValueError:
          continue
      trueorfalse = valid_pattern_or_not_checker(num_dict, temp)
      if trueorfalse:
        print "found valid patterns"
        valid_patterns.append(temp)
  
  thefile =  open("validpatterns.txt", "w")
  easy = open("easy.txt", "w")
  medium = open("medium.txt", "w")
  hard = open("hard.txt", "w")
  randomf = open("random.txt", "w")
  
  random_lines = []
  rcount = 0
  ecount = 0
  mcount = 0
  hcount = 0
  
  COUNTNEEDED = 30
  for item in valid_patterns:
    try:
      if(random() >= .5):
        if rcount <= COUNTNEEDED:
          random_lines.append(item)
          rcount += 1
      thefile.write("%s\n" % ''.join(str(e) for e in item))
      
      if len(item) <= 4:
        if ecount <= COUNTNEEDED:
          easy.write("%s\n" % ''.join(str(e) for e in item))
          ecount += 1
      elif len(item) <= 6 & len(item) > 4:
        if mcount <= COUNTNEEDED:
          medium.write("%s\n" % ''.join(str(e) for e in item))
          mcount += 1
      elif len(item) > 6:
        if hcount <= COUNTNEEDED:
          hard.write("%s\n" % ''.join(str(e) for e in item)) 
          hcount += 1
      if rcount == COUNTNEEDED & ecount == COUNTNEEDED & mcount == COUNTNEEDED & hcount == COUNTNEEDED:
        break

    except TypeError:
      continue
  
  for l in random_lines:
    try:
      randomf.write("%s\n" % ''.join(str(e) for e in l))
    except TypeError:
      continue
 

if __name__ == "__main__":
  get_patterns_in_2D_list()
         
