CC = gcc
CFLAGS := -Wall -g -fPIC -std=gnu99 $(shell pkg-config --cflags --libs mono-2)

all: Arma2Net.so RVExtensionTest

Arma2Net.o: Arma2Net.c Arma2Net.h
	$(CC) $< -o $@ -c $(CFLAGS)

Arma2Net.so: Arma2Net.o
	$(CC) $^ -o $@ -shared $(CFLAGS)

RVExtensionTest.o: RVExtensionTest.c Arma2Net.h
	$(CC) $< -o $@ -c $(CFLAGS)

RVExtensionTest: RVExtensionTest.o Arma2Net.o
	$(CC) $^ -o $@ $(CFLAGS)

.PHONY: clean
clean:
	-rm *.so *.o RVExtensionTest
