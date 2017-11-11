count = [0] * 10

def middle(a, b):
    if (a+b) % 2 != 0:
        return None
    mid = (a+b)/2
    if mid == 5:
        return mid
    if a%3 == b%3:
        return mid
    if (a+b) % 3 == 1:
        return mid
    return None

def poss(l=[]):
    if len(l) == 0:
        for x in range(1, 10):
            yield x
        return
    for x in range(1,10):
        mid = middle(x, l[-1])
        if (x not in l) and (mid in l or mid == None):
           yield x

def comb(l=[]):
    ll = len(l);
    if ll == 10:
        return
    global count
    t = list(poss(l))
    # print t
    if ll >= 4:
        count[ll] = count[ll] + 1
    for x in t:
        comb(l + [x])

#print list(poss([x]))

comb()

print count
print "The number of all possibilities is : %i" % sum(count)
