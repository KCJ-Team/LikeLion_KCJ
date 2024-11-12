using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController
{
    private readonly PlayerCharacterController _player;
    private readonly PlayerData _playerData;
    private readonly Dictionary<KeyCode, float> _skillCooldowns = new Dictionary<KeyCode, float>();
    private readonly Dictionary<KeyCode, Skill> _activeSkills = new Dictionary<KeyCode, Skill>();
    private bool _isSkillActive;
    private readonly Animator _animator;

    public PlayerSkillController(PlayerCharacterController player, PlayerData playerData)
    {
        _player = player;
        _playerData = playerData;
        _animator = player.GetComponent<Animator>();
    }

    public void HandleSkills()
    {
        HandleSkillInputs();
        UpdateCooldowns();
        UpdateSkillState();
    }

    private void HandleSkillInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isSkillActive = true;
            UseSkill(KeyCode.Q, _playerData.currentQSkill?.skillData.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isSkillActive = true;
            UseSkill(KeyCode.E, _playerData.currentESkill?.skillData.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseSkill(KeyCode.LeftShift, _playerData.currentBuff?.buffData.gameObject);
        }

        // 활성화된 스킬 확인 및 상태 업데이트
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            KeyCode releasedKey = Input.GetKeyUp(KeyCode.Q) ? KeyCode.Q : KeyCode.E;
            
            // 해당 키의 스킬이 이미 파괴되었는지 확인
            if (_activeSkills.TryGetValue(releasedKey, out Skill skill))
            {
                if (skill == null || skill.gameObject == null)
                {
                    _activeSkills.Remove(releasedKey);
                }
            }

            // 다른 활성 스킬이 없다면 스킬 상태를 false로 설정
            bool hasActiveSkills = false;
            foreach (var activeSkill in _activeSkills.Values)
            {
                if (activeSkill != null && activeSkill.gameObject != null)
                {
                    hasActiveSkills = true;
                    break;
                }
            }
            
            if (!hasActiveSkills)
            {
                _isSkillActive = false;
            }
        }
    }

    private void UpdateSkillState()
    {
        // 활성화된 스킬 상태 검증
        bool hasActiveSkills = false;
        var keysToRemove = new List<KeyCode>();

        foreach (var kvp in _activeSkills)
        {
            if (kvp.Value == null || kvp.Value.gameObject == null)
            {
                keysToRemove.Add(kvp.Key);
            }
            else
            {
                hasActiveSkills = true;
            }
        }

        // 파괴된 스킬 참조 제거
        foreach (var key in keysToRemove)
        {
            _activeSkills.Remove(key);
        }

        // 실제 상태 반영
        _isSkillActive = hasActiveSkills;
        _animator.SetBool("IsSkill", _isSkillActive);
    }

    private void UseSkill(KeyCode key, GameObject skillPrefab)
    {
        if (skillPrefab == null || IsSkillOnCooldown(key)) return;

        // 기존 스킬 정리
        if (_activeSkills.TryGetValue(key, out Skill existingSkill))
        {
            if (existingSkill != null && existingSkill.gameObject != null)
            {
                Object.Destroy(existingSkill.gameObject);
            }
            _activeSkills.Remove(key);
        }

        // 마우스 방향으로 회전
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, _player.transform.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 direction = pointToLook - _player.transform.position;
            direction.y = 0f;
            
            if (direction.magnitude > 0.1f)
            {
                _player.transform.rotation = Quaternion.LookRotation(direction.normalized);
            }
        }

        // 새 스킬 생성 및 초기화
        GameObject skillObject = Object.Instantiate(skillPrefab, _player.transform.position, _player.transform.rotation);
        Skill skill = skillObject.GetComponent<Skill>();
        
        if (skill != null)
        {
            skill.Initialize(_player);
            if (skill.TryExecute())
            {
                _skillCooldowns[key] = skill.Cooldown;
                _activeSkills[key] = skill;
            }
            else
            {
                Object.Destroy(skillObject);
            }
        }
    }

    private void UpdateCooldowns()
    {
        var keysToUpdate = new List<KeyCode>(_skillCooldowns.Keys);
        foreach (KeyCode key in keysToUpdate)
        {
            if (_skillCooldowns[key] > 0)
            {
                _skillCooldowns[key] -= Time.deltaTime;
                if (_skillCooldowns[key] <= 0)
                {
                    _skillCooldowns.Remove(key);
                }
            }
        }
    }

    public bool IsSkillOnCooldown(KeyCode key)
    {
        return _skillCooldowns.TryGetValue(key, out float cooldown) && cooldown > 0;
    }

    public float GetSkillCooldown(KeyCode key)
    {
        return _skillCooldowns.TryGetValue(key, out float cooldown) ? cooldown : 0f;
    }

    public void CleanupSkills()
    {
        foreach (var skill in _activeSkills.Values)
        {
            if (skill != null && skill.gameObject != null)
            {
                Object.Destroy(skill.gameObject);
            }
        }
        _activeSkills.Clear();
        _isSkillActive = false;
        _animator.SetBool("IsSkill", false);
    }
}