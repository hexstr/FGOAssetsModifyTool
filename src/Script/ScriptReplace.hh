#ifndef SCRIPTREPLACE_HEADER
#define SCRIPTREPLACE_HEADER

#include "Hook/HookTemplate.hh"

class ScriptReplace : public HookTemplate {
public:
    virtual void Start() override final;
};

#endif // ScriptReplace.hh