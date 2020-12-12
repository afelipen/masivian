
public class PrimeNumbersManager {	 	 	  	
	public static int[] generate(int numMaxPrimes) {		 
		int[] numPrimes = new int[numMaxPrimes];
		setInitialFirtsNumber(numPrimes);
		processNumberPrimes(numPrimes);		
		return numPrimes;
	}

	private static void setInitialFirtsNumber(int[] numPrimes ) {
		numPrimes[0] = 2;
	}

	private static void processNumberPrimes(int[] numPrimes ) {				
		int index =1;		 
		for(int number = 3;index < numPrimes.length; number +=2) {			 
			 if(isNumberPrime(number)) {
				 numPrimes[index] = number;
				 index++;
			 }						 
		 }
	}
	
	private static boolean isNumberPrime(int number) {
		int outWhile =0;
		int divider = 2;	
		int residue;
		while((divider < number) & (outWhile ==0)) {
			residue= (number % divider);
			if(residue ==0) {
				outWhile  = 1;
			}
			else {
				divider++;
			}			
		}
		if(outWhile==0) {
			return true;
		}		
		
		return false;
	}
}
