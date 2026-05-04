/* 
 * File:   asservissement.h
 * Author: E306-PC2
 *
 * Created on 29 avril 2026, 18:29
 */

#ifndef ASSERVISSEMENT_H
#define	ASSERVISSEMENT_H
#define M_TO_PERCENT 30

typedef struct _PidCorrector
{
double Kp;
double Ki;
double Kd;
double erreurProportionelleMax;
double erreurIntegraleMax;
double erreurDeriveeMax;
double erreurIntegrale;
double epsilon_1;
double erreur;
//For Debug only
double corrP;
double corrI;
double corrD;
}PidCorrector;

extern double proportionelleMax;
extern double integralMax;
extern double deriveeMax;

extern PidCorrector PidX;
extern PidCorrector PidTheta;

void UpdateAsservissement();
double Correcteur(volatile PidCorrector* PidCorr, double erreur);
void PWMSetSpeedConsignePolaire(float vitesseLineaire, float vitesseAngulaire);

#define DISTROUES 0.218

#ifdef	__cplusplus
extern "C" {
#endif



#ifdef	__cplusplus
}
#endif

#endif	/* ASSERVISSEMENT_H */

