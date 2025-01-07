from tkinter import *
import time
import datetime
import math
#ぱいそんのたいまー

root = Tk()

buff = StringVar()
buff.set('')
Label(textvariable = buff).pack()


ibe='invaid PST-PDT ambious'
s = '2024-03-09T17:00:00Z'
ss = '2024-11-02T16:00:00Z'
timefm='%Y/%m/%d %H:%M:%S %Z'

s=s.replace('Z', '+00:00')
ss=ss.replace('Z', '+00:00')

st  = datetime.datetime.fromisoformat(s)
en  = datetime.datetime.fromisoformat(ss)

sttmp=st.timestamp()
st=datetime.datetime.fromtimestamp(sttmp)
ts = st.strftime(timefm)
entmp=en.timestamp()
en=datetime.datetime.fromtimestamp(entmp)
te = en.strftime(timefm)
span= abs(en-st)


def dtime(dt):
    if dt<0:
            return "0日0時間0分"
    dt=abs(dt)
    seconds  = math.floor((dt / 10) % 60)
    minutes  = math.floor((dt / 60) % 60)
    hours    = math.floor((dt / 3600) % 24)
    days     = math.floor(dt / 86400)
    tmp = str(days) +"日" +str(hours)+"時間"+str(minutes) +"分"
    return tmp

# 時刻の表示
def show_time():
    nn=time.time()
    elapsed=dtime(nn-sttmp)
    left= dtime(entmp-nn)
    x = (nn-sttmp)/abs(entmp-sttmp)*100
    n = 2
    y = math.floor(x * 10 ** n) / (10 ** n)
    progress = y
    if y>100:
             y=100
    if y<0:
             y=0
             
    tm=time.strftime(timefm)
    buff.set(ibe +'\n'+ tm+'\n' +ts +'\n'+te+"\n" +str(span)+"\n"+elapsed+"\n"+left+"\n"+str(y)+"%")
    root.after(1000, show_time)
    #1000msの後にshow_time関数を呼び出しています


show_time()
root.mainloop()