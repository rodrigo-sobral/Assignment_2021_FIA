# Project_FIA_2021
 

___
## map1a

func. ativacao w / r | weight w / r | angle sens w / r | range sens w / r | lim. inf. out(y) w / r | lim. sup. out(y) w / r | lim. inf. stren(x) w / r | lim. sup. stren(x) w / r | gauss micro w / r | gauss sigma w / r | tempo (s)
-- | -- | -- | -- | -- | -- | -- | -- | -- | -- | -- 
linear / linear | -1.5 / 2  | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / --   | -- / --   | 7
linear / gauss  | -1.5 / 2  | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / 0.5  | -- / 0.12 | 7
linear / log    | -0.75 / 1 | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / --   | -- / --   | 9
|
gauss / gauss  | -1.5 / 2  | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | 0.5 / 0.5 | 0.12 / 0.12 | 5
gauss / linear | -0.75 / 1 | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | 0.5 / --  | 0.12 / --   | 8
gauss / log    | -1.5 / 2  | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | 0.5 / --  | 0.12 / --   | 4
|
log / log    | -1.5 / 2    | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / --  | -- / --    | 4
log / gauss  | -0.5 / 2    | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / 0.5 | -- / 0.12  | 6
log / linear | -0.5 / 0.75 | 10 / 10 | 10 / 30 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / --  | -- / --    | 9


___
## map1b

func. ativacao w / r | weight w / r | angle sens w / r | range sens w / r | lim. inf. out(y) w / r | lim. sup. out(y) w / r | lim. inf. stren(x) w / r | lim. sup. stren(x) w / r | gauss micro w / r | gauss sigma w / r | tempo (s)
-- | -- | -- | -- | -- | -- | -- | -- | -- | -- | --
linear / linear | -0.6 / 2.5 | 10 / 10 | 10 / 50 | 0 / 0    | 1 / 1   | 0 / 0   | 1 / 1 | -- / --  | -- / --   | 10 
linear / gauss  | -0.5 / 2   | 10 / 10 | 10 / 50 | 0 / 0    | 0.4 / 1 | 0 / 0   | 1 / 1 | -- / 0.6 | -- / 0.12 | 10 
linear / log    | -0.5 / 2   | 10 / 10 | 10 / 50 | 0 / 0.05 | 1 / 0.3 | 0 / 0.1 | 1 / 1 | -- / --  | -- / --   | 10  
|
gauss / gauss  | -0.5 / 2 | 10 / 10 | 10 / 50 | 0 / 0     | 0.6 / 1 | 0 / 0    | 1 / 1 | 0.7 / 0.3 | 0.12 / 0.12 | 9 
gauss / linear | -0.5 / 2 | 10 / 10 | 10 / 50 | 0.1 / 0   | 1 / 1   | 0 / 0    | 1 / 1 | 0.8 / --  | 0.12 / --   | 10  
gauss / log    | -0.5 / 2 | 10 / 10 | 10 / 50 | 0.1 / 0.1 | 1 / 1   | 0 / 0.05 | 1 / 1 | 0.8 / --  | 0.12 / --   | 9 
|
log / log    | -0.5 / 3 | 10 / 10 | 10 / 50 | 0 / 0.15 | 0.6 / 1 | 0.4 / 0.25 | 1 / 1       | --/ --   | -- / --   | 8
log / gauss  | -0.5 / 2 | 10 / 10 | 10 / 50 | 0.2 / 0  | 1 / 1   | 0 / 0      | 0.25 / 0.2  | -- / 0.4 | -- / 0.12 | 11 
log / linear | -0.5 / 2 | 10 / 10 | 10 / 50 | 0.25 / 0 | 1 / 1   | 0 / 0      | 1 / 0.2     | -- / --  | -- / --   | 10 


___
## map2a

`Nota: Aqui não temos ws, logo só precisamos da função de ativação dos sensores de recursos (weight w=0 ; weight r=1)`

funcao de ativacao r | weight w / r | angle sens w / r | range sens w / r | limite inf out(y) | limite sup out(y) | limite inf stren(x) | limite sup stren(x) | gauss micro | gauss sigma | tempo(s)
-- | -- | -- | -- | -- | -- | -- | -- | -- | -- | --
linear | 0 / 1 | 10 / 10 | 10 / 30 | 0.1 | 0.2 | 0 | 0.02 | --   | --    | 7
gauss  | 0 / 1 | 10 / 10 | 10 / 30 | 0.1 | 0.2 | 0 | 0.03 | 0.5  | 0.12  | 7
log    | 0 / 1 | 10 / 10 | 10 / 30 | 0.1 | 0.2 | 0 | 0.02 | --   | --    | 7



___
## map2b

func. ativacao w / r | weight w / r | angle sens w / r | range sens w / r | lim. inf. out(y) w / r | lim. sup. out(y) w / r | lim. inf. stren(x) w / r | lim. sup. stren(x) w / r | gauss micro w / r | gauss sigma w / r | tempo (s)
-- | -- | -- | -- | -- | -- | -- | -- | -- | -- | --
linear / linear | -0.85 / 2.5 | 5 / 10 | 10 / 100 | 0 / 0.1 | 1 / 1 | 0 / 0.1 | 1 / 1 | -- / -- | -- / -- | 18
linear / gauss
linear / log
|
gauss / gauss
gauss / linear
gauss / log
|
log / log
log / gauss
log / linear | -0.5 / 2 | 10 / 10 | 10 / 50 | 0 / 0 | 0.5 / 1 | 0 / 0 | 1 / 1 | -- / -- | -- / -- | 28


___
## map_gus

func. ativacao w / r | weight w / r | angle sens w / r | range sens w / r | lim. inf. out(y) w / r | lim. sup. out(y) w / r | lim. inf. stren(x) w / r | lim. sup. stren(x) w / r | gauss micro w / r | gauss sigma w / r | tempo (s)
-- | -- | -- | -- | -- | -- | -- | -- | -- | -- | --
linear | linear
linear | gauss
linear | log
|
gauss | gauss
gauss | linear
gauss | log
|
log | log
log | gauss
log | linear
