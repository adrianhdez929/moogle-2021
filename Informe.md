# Informe Moogle!

### Estudiante: Adrian Hernandez Santos
#### Grupo: C-121

## Algoritmo Principal

### En este proyecto utilizo la formula simple TF x IDF (Term Frequency, Inverse Document Frequency), para asignar un peso a cada documento, basado en la ocurrencia de palabras de una determinada "query" (busqueda) introducida por el usuario

### Primeramente seleccionamos los archivos contenidos en la carpeta "Content" del directorio raiz del proyecto, utilizando los metodos de la clase estatica Directory, intentando buscar un path relativo, independientemente de la localizaciond el proyecto, el sistema operativo, etc... buscando una forma de generalizar el uso del buscador. Luego, con cada archivo creamos una instancia de la clase propia Document, la cual guarda los valores asociados a cada documento, en un objeto, lo cual nos permite acceder a ellos por referencia, en vez de tener varios arrays con dichos datos indexados, lo cual nos dificulta el hecho de tener que mantener una especie de "constancia" a la hora de acceder a cada indice asociado a cada documento. Luego, la clase Query, guarda dichos valores relativos a la busqueda del usuario. Al tener estos datos (coleccion de documentos y query), procedemos a crear una matriz de MxN (M: cantidad de palabras del query, N: cantidad de documentos) donde el valor de M[i, j] representa el peso (W[i,j]) representado como TF x IDF de dicha palabra en el documento dado). Luego de realizar eso, procedemos a sumar todas las filas, y quedarnos con el vector resultante de dimension igual a la cantidad de documentos de nuestra coleccion, los cuales ordenamos de manera ascendente, asignando como score el peso de cada documento, que se mostrara solamente en caso de que su peso sea mayor que 0. Por otra parte, el snippet que se devuelve se hace de forma tal que si el query esta contenido en el documento, se devuelve ese pedazo de texto, sino, se devuelven todas las palabras del query que esten contenidas en dicho documento.

