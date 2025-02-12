using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootBulletTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private void Awake() {
        
        if(TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction)) {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }
    }

    private void Start() {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, System.EventArgs e) {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, System.EventArgs e) {
        EquipRifle();
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e) {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e) {
        animator.SetBool("IsWalking", false);

    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e) {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootBulletTransform.position, Quaternion.identity);

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootBulletTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    private void EquipSword() {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle() {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }

}
